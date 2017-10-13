var OpenLinkPlugin = {  
    openLink: function(link)
    {
        var url = Pointer_stringify(link);
        document.onmouseup = function()
        {
            document.onmouseup = null;
            window.open(url,'_parent');
        }
    }
};

mergeInto(LibraryManager.library, OpenLinkPlugin);  