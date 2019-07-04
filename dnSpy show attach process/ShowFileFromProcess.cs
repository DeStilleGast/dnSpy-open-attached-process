using dnSpy.Contracts.App;
using dnSpy.Contracts.Debugger;
using dnSpy.Contracts.Documents;
using dnSpy.Contracts.Documents.Tabs;
using dnSpy.Contracts.Documents.TreeView;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace dnSpy_show_attach_process {

    // https://github.com/0xd4d/dnSpy/blob/master/Extensions/dnSpy.Debugger/dnSpy.Debugger/Impl/SwitchToDebuggedProcess.cs

    [Export(typeof(IDbgManagerStartListener))]
    class ShowFileFromProcess : IDbgManagerStartListener {


        [Import]
        public IDocumentTabService documentTabService;
        [Import]
        public IAppWindow appWindow;

        private DbgProcess currentProcess;
        private DsDocumentNode currentDoc;

        public void OnStart(DbgManager dbgManager) {
            dbgManager.CurrentProcessChanged += DbgManager_CurrentProcessChanged;
            dbgManager.DelayedIsRunningChanged += DbgManager_DelayedIsRunningChanged;
        }

        private void DbgManager_CurrentProcessChanged(object sender, DbgCurrentObjectChangedEventArgs<DbgProcess> e) {
            if (!e.CurrentChanged)
                return;
            var newProcess = ((DbgManager)sender).CurrentProcess.Current;
            if (!(newProcess is null))
                currentProcess = newProcess;
        }

        private void DbgManager_DelayedIsRunningChanged(object sender, EventArgs e) {
            var process = currentProcess;

            if (process is null)
                process = ((DbgManager)sender).Processes.FirstOrDefault(a => a.State == DbgProcessState.Running);

            // Fails if the process hasn't been created yet (eg. the engine hasn't connected to the process yet)
            if (process is null) {
                return;
            }

            process.Closed += TryToRemoveNode;

            // Add document to file tree
            AddDocument(process.Filename);
        }

        private void AddDocument(string filename) {
            var doc = DsDocumentInfo.CreateDocument(filename);
            var idoc = documentTabService.DocumentTreeView.DocumentService.CreateDocument(doc, filename);
            var node = documentTabService.DocumentTreeView.CreateNode(null, idoc);

            currentDoc = node;

            var dis = appWindow.MainWindow.Dispatcher; // application crashes when it isn't run from the main thread
            dis.BeginInvoke(System.Windows.Threading.DispatcherPriority.Input, new Action(() => {
                documentTabService.DocumentTreeView.AddNode(node, -1); // most likely down at the bottem of the list

                var docNode = documentTabService.DocumentTreeView.FindNode(node);

                docNode.TreeNode.TreeView.SelectItems(new List<DocumentTreeNodeData>() { docNode }.AsEnumerable());
            }));
        }

        private void TryToRemoveNode(object sender, EventArgs e) {
            try {
                if (currentDoc != null) {
                    documentTabService.DocumentTreeView.Remove(new List<DsDocumentNode>() { currentDoc }.AsEnumerable());
                }
            } catch { }
        }
    }
}
