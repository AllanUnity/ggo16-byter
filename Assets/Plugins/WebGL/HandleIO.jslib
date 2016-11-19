// Required for persistence in WebGL

var HandleIO = {
     SyncFiles: function() {
         FS.syncfs(false, function(err) {
		if (err) {
         		console.error("FS.syncfs failed due to:");
			console.error(err);
		}
         });
     }
 };
 
 mergeInto(LibraryManager.library, HandleIO);
