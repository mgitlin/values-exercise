var OpenWindowPlugin = {  
    openWindow: function(link)
    {
        var url = Pointer_stringify(link);
        document.onmouseup = function()
        {
            document.onmouseup = null;
            var windowContent = '<!DOCTYPE html>';
		    windowContent += '<html>'
		    windowContent += '<head><title>Values Exercise Results</title></head>';
		    windowContent += '<body>'
		    windowContent += '<img src="' + url + '">';
		    windowContent += '<p>Click <a href="' + url + '" download="results.png">here</a> to download the image. You may also print this page for reference.</p>';
		    windowContent += '</body>';
		    windowContent += '</html>';
		    var Win = window.open(url,'_blank','width=1000,height=700');
		    Win.document.write(windowContent);
		    Win.focus();
        }
    }
};

mergeInto(LibraryManager.library, OpenWindowPlugin);  