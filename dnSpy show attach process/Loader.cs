using dnSpy.Contracts.Extension;
using System.Collections.Generic;

namespace dnSpy_show_attach_process {

    [ExportExtension]
    public class Loader : IExtension {
        public ExtensionInfo ExtensionInfo => new ExtensionInfo() {
            Copyright = "Copyright 2019 DeStilleGast",
            ShortDescription = "Opens the file from the attach process dialog"
        };

        public IEnumerable<string> MergedResourceDictionaries {
            get {
                yield break;
            }
        }

        public void OnEvent(ExtensionEvent @event, object obj) { }
    }
}
