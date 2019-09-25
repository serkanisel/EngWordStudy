$(function(){
    RootURL="http://localhost:8010/EWS.Web/";

    checkSession();

    function f() {
        alert("dana");
    }
    
    function checkSession(){
        $.ajax({
            url: RootURL + 'EWSActions/CheckSession',
            type: 'POST',
            async: true,
            success: function (text) {
                if(text==false)
                {
                    $("#loginPart").show();
                    $("#wordPart").hide();
                }
                else
                {
                    $("#loginPart").hide();
                    $("#wordPart").show();
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                alert("Bir Hata var");
            }
        });
    }
    
    function getUserSession(){
        $("#mainPart").load("Word.html");
    }

    $(document)
    .ajaxStart(function () {
        ajaxStart();
    })
    .ajaxStop(function () {
        ajaxEnd();
    });


    
        chrome.storage.sync.get('EWSWord',function(budget){
            if(budget.EWSWord!=null)
            {
                $("#txtBody").val(budget.EWSWord);
                $("#txtSentenceBody").val(budget.EWSWord);
                getWordMean();

                var link_google='https://translate.google.com/#view=home&op=translate&sl=en&tl=tr&text=' + $("#txtBody").val();
                var link_ss='https://www.seslisozluk.net/'+$("#txtBody").val();+'-nedir-ne-demek/' ; 

                $("#anc_googleTranslate").attr("href", link_google);
                $("#anc_seslisozlukTranslate").attr("href", link_ss);
            }
        });
    

    $("#btnLogin").click(function(){
        login();
    });

    $("#btnGetMean").click(function(){
        getWordMean();
    });

    $("#btnSearch").click(function(){
        SearchWord();
    });
    

    $("#btnSaveWord").click(function(){
        saveNewWord();
    });

    $("#btnSaveSentence").click(function(){
        saveNewSentence();
    });
    
    $("#btnScriptTry").click(function(){
        tryScript();
    });
    
    function ajaxStart() {
        $('#loadingDiv').css("display", "block");
    }

    function ajaxEnd() {
        $('#loadingDiv').css("display", "none");
    }
    
    function getWordMean(){
        var ewsword=$("#txtBody").val();

        if(ewsword==null || ewsword=="")
            return;

        $.ajax({
            url: RootURL + 'EWSActions/TranslateTextJson',
            type: 'POST',
            data: { TranslateText: ewsword },
            async: true,
            success: function (text, data) {
                chrome.storage.sync.set({'EWSWord':ewsword},function(){
            
                });
                        
                $("#txtMean").val(text["Description"]);
                $("#txtSentenceMean").val(text["Description"]);
            },
            error: function (jqXhr, textStatus, errorThrown) {
                alert("Bir Hata var");
            }
        });
    };

    function SearchWord(){
        var ewsword=$("#txtWord").val();

        if(ewsword==null || ewsword=="")
            return;

        $.ajax({
            url: RootURL + 'EWSActions/TranslateTextJson',
            type: 'POST',
            data: { TranslateText: ewsword },
            async: true,
            success: function (text, data) {
                $("#txtMean").val(text["Description"]);
                $("#txtSentenceMean").val(text["Description"]);
            },
            error: function (jqXhr, textStatus, errorThrown) {
                alert("Bir Hata var");
            }
        });
    };

    function saveNewWord(){
        var ewsword=$("#txtBody").val();
        var ewsDesc=$("#txtMean").val();
        $.ajax({
            url: RootURL + 'EWSActions/WordSave',
            type: 'POST',
            data: { wordbody: ewsword, description:ewsDesc },
            async: true,
            success: function (text, data) {
                $("#wordResult").html(text);
            },
            error: function (jqXhr, textStatus, errorThrown) {
                alert("Bir Hata Var");
            }
        });
    };

    function saveNewSentence(){
        var ewsword=$("#txtBody").val();
        var ewsDesc=$("#txtMean").val();
        $.ajax({
            url: RootURL + 'EWSActions/SaveSentence',
            type: 'POST',
            data: { sentenceBody: ewsword, sentenceMean:ewsDesc },
            async: true,
            success: function (text, data) {
                $("#wordResult").html(text);
            },
            error: function (jqXhr, textStatus, errorThrown) {
                alert("Bir Hata Var");
            }
        });
    };

    function tryScript(){
        $.ajax({
            url: RootURL + 'EWSActions/ScriptReturn',
            type: 'POST',
            async: true,
            success: function (text) {
                $("#wordResult").html(text);
            },
            error: function (jqXhr, textStatus, errorThrown) {
                alert("Bir Hata var");
            }
        });
    };

    
    function login(){
        var loginName=$("#txtLoginName").val();
        var pass=$("#txtPass").val();

        $.ajax({
            url: RootURL + 'EWSActions/Login',
            type: 'POST',
            async: true,
            data: { loginName: loginName, pass:pass },
            success: function (text) {
                if(text==true)
                {
                    $("#mainPart").load("Word.html");
                }
                else{
                    alert("Kullanıcı Adı Şifre Hatalı");
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                alert("Bir Hata var");
            }
        });
    }
    
})

