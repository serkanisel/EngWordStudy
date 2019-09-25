var contextMenuItem = {
    "id":"EWS",
    "title":"EWS",
    "contexts": ["selection"]
};

chrome.contextMenus.create(contextMenuItem);

chrome.contextMenus.onClicked.addListener(function(clickData){
    if(clickData.menuItemId=="EWS" && clickData.selectionText){
        var word=clickData.selectionText;

        chrome.storage.sync.set({'EWSWord':word},function(){
            
        });

    }
});