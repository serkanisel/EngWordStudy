var RootURL = "/EWS.Web/";
var EmptyGUID = "00000000-0000-0000-0000-000000000000";


function yeniKelimeKaydet(resultDiv) {
    var body = $("#txtWordBody").val();
    var txtDesc = $("#txtDescription").val();
    //var samp = $("#txtSample").val();
    //var sampMean = $("#txtSampleMean").val();

    var divName = "#" + resultDiv;
    $.ajax({
        url: RootURL + 'EWSWords/NewWord',
        type: 'POST',
        data: { wordbody: body, description: txtDesc },
        async: false,
        success: function (text, data) {
            $(divName).html(text);

        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, 'error');
        }
    });
}

function SearchWord() {
    var body = $("#txtBody").val();
    if (body == "")
        return;

    $.ajax({
        url: RootURL + 'EWSWords/SearchWord',
        type: 'POST',
        data: { wordBody: body },
        async: false,
        success: function (text, data) {
            $("#SearchList").fadeIn();
            $("#divNewWord").fadeOut();
            $("#SearchList").html(text);
            buttonEvent();

        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, 'error');
        }
    });
}

function wordList(_listUN) {
    var listUN;
    if (_listUN == null)
        listUN = $("#listUN").val();
    else
        listUN = _listUN;
    $.ajax({
        url: RootURL + 'EWSWords/GetWordList',
        type: 'POST',
        data: { ListID: listUN },
        async: false,
        success: function (text, data) {
            $("#wordList").html(text);
        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, 'error');
        }
    });

}

function buttonEvent() {
    $(".wordClass").click(function () {
        var listUN = $("#listUN").val();
        var wordUN = $(this).attr('data-content');


        $.ajax({
            url: RootURL + 'EWSWords/AddWordToList',
            type: 'POST',
            data: { listUN: listUN, wordUN: wordUN },
            async: false,
            success: function (text, data) {
                $("#addWordResult").html(text);
                wordList();
            },
            error: function (jqXhr, textStatus, errorThrown) {
                Message(errorThrown, 'error');
            }
        });
    });
}

function yeniKelimeEkle() {
    $("#divNewWord").fadeIn();
}

function ListedenCikar(UN) {
    $.ajax({
        url: RootURL + 'EWSLists/ListedenCikar',
        type: 'POST',
        data: { UN: UN },
        async: false,
        success: function (text, data) {
            $("#addWordResult").html(text);
            $("#divListWord_" + UN).remove();
        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, 'error');
        }
    });
}

function Message(message, type) {
    if (type === "error") {
        $("#msg").html("<i class='fa fa-exclamation-triangle'></i> " + message);
        $("#msg").removeClass("alert-info");
        $("#msg").removeClass("alert-success");
        $("#msg").removeClass("alert-warning");

        $("#msg").addClass("alert alert-danger");
    }
    else if (type === "success") {
        $("#msg").html("<i class='fa fa-check'></i> " + message);

        $("#msg").removeClass("alert-info");
        $("#msg").removeClass("alert-danger");
        $("#msg").removeClass("alert-warning");

        $("#msg").addClass("alert alert-success");
    }
    else if (type === "info") {
        $("#msg").html("<i class='fa fa-info'></i> " + message);

        $("#msg").removeClass("alert-danger");
        $("#msg").removeClass("alert-success");
        $("#msg").removeClass("alert-warning");

        $("#msg").addClass("alert alert-info");
    }

    $("#message").fadeIn();
    //$("#message").css("right", "30px");
    $('#message').delay(3000).fadeOut();

}

function WordDoesntExists(wordBody) {
    $("#SearchList").slideUp();
    yeniKelimeEkle();
    $("#txtWordBody").val(wordBody);
}

function getWordDetails(UN) {

    $.ajax({
        url: RootURL + 'EWSWords/GetWordDetailByUN',
        type: 'POST',
        data: { UN: UN },
        async: false,
        success: function (text, data) {
            $("#footerWordDescription").height("350");
            $("#footerWordDescription").html(text);
            $("#btnFooterKapat").show();
        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, 'error');
        }
    });
}

function GetTodayDate() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getFullYear();

    if (dd < 10) {
        dd = '0' + dd;
    }

    if (mm < 10) {
        mm = '0' + mm;
    }

    today = mm + '/' + dd + '/' + yyyy;

    return today;
}
function GetTodayDateYear() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getFullYear();


    return yyyy;
}

function footerkapat() {
    $("#footerWordDescription").height("50");
    $("#footerWordDescription").html("<div class='col-12 text-center'>" + GetTodayDateYear() + "<span> / </span> Copyright Serkan İşel</div>");
    $("#btnFooterKapat").hide();
}

function readURL(input) {

    if (input.files && input.files[0]) {
        var reader = new FileReader();

        //reader.onload = function (e) {
        //    console.log(e.filename);
        //}
        reader.readAsDataURL(input.files[0]);
        UploadSozluk();
    }
}

function UploadSozluk() {
    var fileUpload = $("#FileUpload").get(0);
    var files = fileUpload.files;

    // Create FormData object
    var fileData = new FormData();

    // Looping over all files and add it to FormData object
    for (var i = 0; i < files.length; i++) {
        fileData.append(files[i].name, files[i]);
    }

    //fileData.append('PersonelID', $("#UN").val());

    $.ajax({
        type: "POST",
        url: RootURL + "EWSFile/UploadSozluk",
        data: fileData,
        contentType: false,
        processData: false,
        success: function (text) {
            $("#fileUploadResult").html(text);
        },
        error: function (error) {
            Message(error, "error");
        }
    });

}

function readURLForFileAnalyze(input) {

    if (input.files && input.files[0]) {
        var reader = new FileReader();

        //reader.onload = function (e) {
        //    console.log(e.filename);
        //}
        reader.readAsDataURL(input.files[0]);
        FileAnalyze();
    }
}

function readURLForFileList(input) {

    if (input.files && input.files[0]) {
        var reader = new FileReader();

        //reader.onload = function (e) {
        //    console.log(e.filename);
        //}
        reader.readAsDataURL(input.files[0]);
        FileUploadForlist();
    }
}

function FileUploadForlist() {
    var fileUpload = $("#FileUploadForList").get(0);
    var files = fileUpload.files;

    // Create FormData object
    var fileData = new FormData();

    // Looping over all files and add it to FormData object
    for (var i = 0; i < files.length; i++) {
        fileData.append(files[i].name, files[i]);
    }

    //fileData.append('PersonelID', $("#UN").val());

    $.ajax({
        type: "POST",
        url: RootURL + "EWSLists/UploadListWithFile",
        data: fileData,
        contentType: false,
        processData: false,
        success: function (text) {
            Message('Liste Kayıt Edildi.', 'success');
            GetSystemLists();
        },
        error: function (error) {
            Message(error, "error");
        }
    });
}

function FileAnalyze() {
    var fileUpload = $("#FileUploadForAnalyze").get(0);
    var files = fileUpload.files;

    // Create FormData object
    var fileData = new FormData();

    // Looping over all files and add it to FormData object
    for (var i = 0; i < files.length; i++) {
        fileData.append(files[i].name, files[i]);
    }

    //fileData.append('PersonelID', $("#UN").val());

    $.ajax({
        type: "POST",
        url: RootURL + "EWSFile/AnalyzeFile",
        data: fileData,
        contentType: false,
        processData: false,
        success: function (text) {
            $("#fileAnalyzeUploadResult").html(text);
            getReadingPart();
        },
        error: function (error) {
            Message(error, "error");
        }
    });

}

function getSelectionText() {
    var TranslateText = "";
    if (window.getSelection) {
        TranslateText = window.getSelection().toString();
    } else if (document.selection && document.selection.type != "Control") {
        TranslateText = document.selection.createRange().text;
    }
    if (TranslateText == "")
        return;

    TextTranslate(TranslateText);


}

function TextTranslate(text) {
    $.ajax({
        url: RootURL + 'EWSWords/TranslateText',
        type: 'POST',
        data: { TranslateText: text },
        async: true,
        success: function (text, data) {
            $("#footerWordDescription").height("250");
            $("#footerWordDescription").html(text);
            $("#btnFooterKapat").show();
        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, 'error');
        }
    });
}

$("#btnLogin").click(function myfunction() {
    login();
});

function loading() {
    $("#loadingDiv").show();
}
function login() {
    loading();
    var username = $("#username").val();
    var pass = $("#password").val();

    $.ajax({
        url: RootURL + 'Account/Login',
        type: 'POST',
        data: { username: username, password: pass },
        async: true,
        success: function (text, data) {
            if (text == "OK") {
                window.location.href = RootURL + "Home/Index";
            }
            else {
                Message("Kullanıcı Adı ve/veya şifre hatalı", "error");
            }
        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message("Kullanıcı Adı ve/veya şifre hatalı", "error");
        }
    });
}

function register() {
    var username = $("#loginnameforregister").val();
    var pass = $("#passwordregister").val();
    var passagain = $("#passwordagain").val();
    var namesurname = $("#namesurname").val();

    if (pass != passagain) {
        Message("Girilen şifreler birbiri ile aynı değil", "error");
        return;
    }
    $.ajax({
        url: RootURL + 'Account/Register',
        type: 'POST',
        data: { username: username, password: pass, namesurname: namesurname },
        async: false,
        success: function (data, textStatus, jQxhr) {
            $("#username").val(username);
            $("#password").val(pass);

            Message("Kayıt işlemi tamamlandı.<br/>Sisteme Giriş Yapılıyor.", "success");

            setTimeout(function () {
                login();
            }, 3000);

        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message("Bir Hata oluştu", "error");
        }
    });
}

function changeReadingPart(UN) {

    $.ajax({
        url: RootURL + 'ReadingPart/GetReadinPartByUNJson',
        type: 'POST',
        data: { UN: UN },
        async: false,
        success: function (text, data) {
            //$("#readinPart").html(text["ReadPart"]);
            $("#txtReadingPart").html(text["ReadPart"]);
            $("#readingPart").html(text["ReadPartHtml"]);
            //$("#readingPartHtml").html(text["ReadPartHtml"]);
        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, 'error');
        }
    });
}

function NewList() {
    var listName = $("#txtListName").val();

    $.ajax({
        url: RootURL + 'EWSLists/SaveNewList',
        type: 'POST',
        data: { listName: listName },
        async: false,
        success: function (text, data) {
            GetUserLists();
        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, "error");
        }
    });
}

function GetUserLists() {

    $.ajax({
        url: RootURL + 'EWSLists/GetUserLists',
        type: 'POST',
        async: false,
        success: function (text) {
            $("#contentBenimListelerim").html(text);
        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, "error");
        }
    });
}

function GetSystemLists() {

    $.ajax({
        url: RootURL + 'EWSLists/GetSystemLists',
        type: 'POST',
        async: false,
        success: function (text) {
            console.log(text);
            $("#listOfSystemList").html(text);
        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, "error");
        }
    });
}

function OgrenmeListemeEkle(ID) {

    $.ajax({
        url: RootURL + 'EWSLists/AddOgrenilecekListeler',
        type: 'POST',
        data: { listUN: ID },
        async: false,
        success: function (Text, data) {
            $("#listIndexResult").html(Text);
            $("#btnOgrListCikar").fadeIn();
            $("#btnOgrListEkle").fadeOut();
        },
        error: function (jqXhr, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });
}

function OgrenmeListemdenCikar(ID) {

    $.ajax({
        url: RootURL + 'EWSLists/CikarOgrenilecekListeler',
        type: 'POST',
        data: { listUN: ID },
        async: false,
        success: function (Text, data) {
            $("#listIndexResult").html(Text);
            $("#btnOgrListCikar").fadeIn();
            $("#btnOgrListEkle").fadeOut();
        },
        error: function (jqXhr, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });
}

function deleteList(ID) {
    $.ajax({
        url: RootURL + 'EWSLists/DeleteList',
        type: 'POST',
        data: { listID: ID },
        async: false,
        success: function (text, data) {
            $("#EWSList_" + ID).remove();
        },
        error: function (jqXhr, textStatus, errorThrown) {
        }
    });
}

function deleteReadingPart(ID) {
    Message("şu anda yok öyle bişey yeğen", "info");
    //$.ajax({
    //    url: RootURL+ 'ReadingPart/DeleteList',
    //    type: 'POST',
    //    data: { listID: ID },
    //    async: false,
    //    success: function (text, data) {
    //        $("#EWSList_" + ID).remove();
    //    },
    //    error: function (jqXhr, textStatus, errorThrown) {
    //    }
    //});
}

function getReadingPart() {

    $.ajax({
        url: RootURL + 'ReadingPart/GetReadingPartList',
        type: 'POST',
        async: false,
        success: function (text) {
            $("#readingPartList").html(text);
        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, "error");
        }
    });
}

function ajaxStart() {
    $('#loadingDiv').css("display", "block");
}

function ajaxEnd() {
    $('#loadingDiv').css("display", "none");
}

function GetListWord(leftright) {
    $.ajax({
        url: RootURL + 'EWSLists/GetListWordBySequenceNo',
        type: 'POST',
        async: true,
        data: { leftright: leftright },
        success: function (text, data) {
            if (text != null) {
                //$("#wordID").html(text["UN"]);
                //$("#wordBody").html(text["WordBody"]);
                //$("#description").html(text["Description"]);
                if (text["CurrentSequence"] != null)
                    $("#currentRow").val(text["CurrentSequence"]);

                if (text["UN"] != null)
                    getWordByUN(text["UN"]);
            }
        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, "error");
        }
    });
}

function getWordByUN(UN) {
    var divName = "#word_" + UN;
    $.ajax({
        url: RootURL + 'EWSWords/GetWordByUN',
        type: 'POST',
        async: true,
        data: { UN: UN },
        success: function (text, data) {
            $("#word").html(text);
        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, "error");
        }
    });
}

function previewSelectedList(listUN) {
    if (listUN == null)
        return;

    $.ajax({
        url: RootURL + 'EWSLists/SelectedList',
        type: 'POST',
        data: { UN: listUN },
        async: true,
        success: function (text, data) {
            GetListWord("");
            $("#listWordCount").html(text);
        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, 'error');
        }
    });

}

function PreviewMultiple(listUN) {
    $.ajax({
        url: RootURL + 'EWSLists/PreviewMultiple',
        type: 'POST',
        async: true,
        data: { UN: listUN },
        success: function (text) {
            $("#divMultiple").html(text);
        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, 'error');
        }
    });
}

function PreviewTable(listUN) {
    $.ajax({
        url: RootURL + 'EWSLists/PreviewTable',
        type: 'POST',
        async: true,
        data: { UN: listUN },
        success: function (text) {
            $("#divTable").html(text);
        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, 'error');
        }
    });
}

function WordSound() {
    var body = $("#wordBody").html();
    $.ajax({
        url: RootURL + 'EWSWords/WordSound',
        type: 'POST',
        async: false,
        data: { wordBody: body },
        success: function (data, text) {
            $("#ResultOneByOne").html(text);
        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, 'error');
        }
    });
}

function showNavbarMenu() {
    if ($('#navbarMenu').is(':visible')) {
        $("#navbarMenu").slideUp();
    }
    else {
        $("#navbarMenu").slideDown();
    }
}

function ListeYonetimi(ID) {
    console.log(ID);
    window.location.href = RootURL + "EWSLists/AddWordToList?listID=" + ID;
}

function EWS_SetIKnow(wordID) {
    var divName = ".word_" + wordID;

    $.ajax({
        url: RootURL + 'EWSLists/SetIKnow',
        type: 'POST',
        data: { wordID: wordID },
        async: true,
        success: function (text, data) {
            $(divName).html(text);
            setIKnowCount(1);
        },
        error: function (jqXhr, textStatus, errorThrown) {
        }
    });
}

function EWS_RemoveIKnow(wordID) {
    var divName = ".word_" + wordID;
    console.log(wordID);
    $.ajax({
        url: RootURL + 'EWSLists/RemoveIKnow',
        type: 'POST',
        data: { wordID: wordID },
        async: true,
        success: function (text, data) {
            $(divName).html(text);
            setIKnowCount(-1);
        },
        error: function (jqXhr, textStatus, errorThrown) {
        }
    });
}

function EWS_SetIWillLearn(wordID) {
    var divName = ".word_" + wordID;

    $.ajax({
        url: RootURL + 'EWSLists/SetIWillLearn',
        type: 'POST',
        data: { wordID: wordID },
        async: true,
        success: function (text, data) {
            $(divName).html(text);
            setWillLearnCount(1);
        },
        error: function (jqXhr, textStatus, errorThrown) {
        }
    });
}

function EWS_RemoveIWillLearn(wordID) {
    var divName = ".word_" + wordID;

    $.ajax({
        url: RootURL + 'EWSLists/RemoveIWillLearn',
        type: 'POST',
        data: { wordID: wordID },
        async: true,
        success: function (text, data) {
            $(divName).html(text);
            setWillLearnCount(-1);
        },
        error: function (jqXhr, textStatus, errorThrown) {
        }
    });
}

function setIKnowCount(a) {
    var i = parseInt($("#iKnownCount").html()) + parseInt(a);
    $("#iKnownCount").html(i);
}

function setWillLearnCount(a) {
    var i = parseInt($("#willLearnCount").html()) + parseInt(a);
    $("#willLearnCount").html(i);
}
function OkumaParcasiKaydet() {
    var name = $("#readingPartName").val();
    var body = quill.root.innerText;//$("#txtReadingPart").val();
    var html = quill.root.innerHTML;

    $.ajax({
        url: RootURL + 'ReadingPart/NewReadingPart',
        type: 'POST',
        async: true,
        data: { name: name, body: body ,html: html },
        success: function (text, data) {
            $("#fileAnalyzeUploadResult").html(text);
            $("#divNewReadingPart").slideToggle();
            quill.root.innerText = "";
            $("#readingPartName").val("");
            getReadingPart();
        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, "error");
        }
    });
}

function SaveOrnekCumle(wordUN, Sentence, Mean, resultDiv) {
    var result = "#" + resultDiv;
    $.ajax({
        url: RootURL + 'EWSWords/NewSentence',
        type: 'POST',
        data: { body: Sentence, mean: Mean, wordUN: wordUN },
        async: false,
        success: function (text, data) {
            $(result).html(text);
            GetSentencesByWordUN(wordUN, "divSampleSentences");
        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, 'error');
        }
    });
}

function GlobalWordSearch(wordbody, resultDiv) {
    if (wordbody === "")
        return;

    var _resultDiv = "#" + resultDiv;

    $.ajax({
        url: RootURL + 'EWSWords/GlobalSearchWord',
        type: 'POST',
        data: { wordBody: wordbody },
        async: false,
        success: function (text, data) {
            $(_resultDiv).html(text);
        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, 'error');
        }
    });
}

function ListeyeEkle(wordUN) {
    $.ajax({
        url: RootURL + 'EWSWords/GetEWSList',
        type: 'POST',
        data: { wordUN: wordUN },
        async: true,
        success: function (text, data) {
            $("#wordResult").html(text);
            EWSModal("wordResult", "Listeye Ekle");
        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, 'error');
        }
    });



}

function EWSModal(div, title, size) {
    var divname = "#" + div;
    var newDiv = $(divname);

    if (size !== null) {
        $("#dialog").addClass(size);
    }


    $("#EWSModalBody").html(newDiv);
    $(newDiv).show();
    $("#modalTitleEWS").html(title);
    $("#modalEWS").modal();
}

function WordAddToList(WordUN) {
    var listUN = $("#EWSList").val();

    $.ajax({
        url: RootURL + 'EWSWords/AddWordToList',
        type: 'POST',
        data: { listUN: listUN, wordUN: WordUN },
        async: true,
        success: function (text, data) {
            EWSListOfWord(WordUN);
        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, 'error');
        }
    });
}

function EWSListOfWord(wordUN) {
    $.ajax({
        url: RootURL + 'EWSWords/GetEWSListGrid',
        type: 'POST',
        data: { wordUN: wordUN },
        async: true,
        success: function (text, data) {
            $("#listForWord").html(text);
        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, 'error');
        }
    });
}

function WordDetailNewWord(body, desc, resultDiv) {
    var result = "#" + resultDiv;
    var listUN = $("#readPartListUN").val();

    $.ajax({
        url: RootURL + 'EWSWords/NewWordAsListMember',
        type: 'POST',
        data: { wordbody: body, description: desc, ListUN: listUN },
        async: false,
        success: function (text, data) {
            TextTranslate(body);
        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, 'error');
        }
    });
}

function NewSentenceAsListMember(body, mean, resultDiv) {
    var resultDiv = "#" + resultDiv;
    var listUN = $("#readPartListUN").val();

    $.ajax({
        url: RootURL + 'EWSWords/NewSentenceAsListMember',
        type: 'POST',
        data: { body: body, mean: mean, ListUN: listUN },
        async: false,
        success: function (text, data) {
            $(resultDiv).html(text);
        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, 'error');
        }
    });
}

function GetSentencesByWordUN(wordUN, resultDiv) {
    var result = "#" + resultDiv;

    $.ajax({
        url: RootURL + 'EWSWords/GetSentencesByWordUN',
        type: 'POST',
        data: { wordUN: wordUN },
        async: false,
        success: function (text, data) {
            $(result).html(text);
        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, 'error');
        }
    });
}

function GetEkledigimKelimeler(div) {
    var result = "#" + div;
    $.ajax({
        url: RootURL + 'EWSWords/GetEkledigimKelimeler',
        type: 'POST',
        async: true,
        success: function (text) {
            $(result).html(text);
        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, 'error');
        }
    });
}

function table() {
    $(".ewslist").removeClass("col-md-3 col-lg-4");
    $(".ewslist").addClass("col-md-12");
}

function list() {
    $(".ewslist").removeClass("col-md-12");
    $(".ewslist").addClass("col-md-3 col-lg-4");
}

function GetKnownWord() {
    $.ajax({
        url: RootURL + 'EWSWords/GetIKnownWords',
        type: 'POST',
        async: true,
        data: { partial: true },
        success: function (text, data) {
            $("#contentBildigimKelimeler").html(text);
        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, 'error');
        }
    });
}

function GetIWillLearnWords() {
    $.ajax({
        url: RootURL + 'EWSWords/GetWillLearnWords',
        type: 'POST',
        async: true,
        data: { partial: true },
        success: function (text, data) {
            $("#contentOgrenmeListem").html(text);
        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, 'error');
        }
    });
}

//$(document).on("click", function () {
//    $(".tooltip").tooltip('destroy'); // destroy
//});

function UpdateWord(UN, wrdBody, wrdDesc, resultDiv) {
    $.ajax({
        url: RootURL + 'EWSWords/UpdateWord',
        type: 'POST',
        async: true,
        data: { UN: UN, wordbody: wrdBody, description: wrdDesc },
        success: function (text, data) {
            $("#" + resultDiv).html(text);
        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, 'error');
        }
    });
}

function RandomSentence() {
    $.ajax({
        url: RootURL + 'Sentence/Random',
        type: 'GET',
        async: true,
        success: function (text, data) {
            $("#random").html(text);
        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, 'error');
        }
    });
}

function AllSentences() {
    $.ajax({
        url: RootURL + 'Sentence/Liste',
        type: 'GET',
        async: true,
        success: function (text, data) {
            $("#list").html(text);
            
        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, 'error');
        }
    });
}


function UpdateSentence(sentenceUN) {
    var body = $("#sentence_" + sentenceUN).val();
    var txtDesc = $("#sentenceMean_" + sentenceUN).val();

    $.ajax({
        url: RootURL + 'Sentence/UpdateSentence',
        type: 'POST',
        data: { un: sentenceUN, sentenceBody: body, sentenceMean: txtDesc },
        async: true,
        success: function (text, data) {
            $("#sentenceResultDiv").html(text);
        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, 'error');
        }
    });
}

function yeniCumleKaydet(resultDiv) {
    var body = $("#txtSentenceBody").val();
    var txtDesc = $("#txtSenteceMean").val();

    var divName = "#" + resultDiv;
    $.ajax({
        url: RootURL + 'Sentence/NewSentence',
        type: 'POST',
        data: { sentenceBody: body, sentenceMean: txtDesc },
        async: false,
        success: function (text, data) {
            $(divName).html(text);
            $("#txtSentenceBody").val("");
            $("#txtSenteceMean").val("");
        },
        error: function (jqXhr, textStatus, errorThrown) {

            Message(errorThrown, 'error');
        }
    });
}

function searchSentence(resultDiv) {
    var txtSearch = $("#txtSentenceAra").val();

    var divName = "#" + resultDiv;
    $.ajax({
        url: RootURL + 'Sentence/SearchSentence',
        type: 'POST',
        data: { searchSentence: txtSearch },
        async: false,
        success: function (text, data) {
            $(divName).html(text);
        },
        error: function (jqXhr, textStatus, errorThrown) {

            Message(errorThrown, 'error');
        }
    });
}

function getSidebarStatus() {
    $.ajax({
        url: RootURL + 'Home/SidebarStatus',
        type: 'POST',
        async: false,
        success: function (text, data) {
            console.log("ne yaptın" + text);
            $("#sideBarSwitch").val(text);
        },
        error: function (jqXhr, textStatus, errorThrown) {

            Message(errorThrown, 'error');
        }
    });
}

function SetSidebarStatus(stat) {
    $.ajax({
        url: RootURL + 'Home/SidebarStatusSet',
        type: 'POST',
        async: false,
        data: { status: stat },
        success: function (text, data) {
            $("#sideBarSwitch").val(text);
        },
        error: function (jqXhr, textStatus, errorThrown) {

            Message(errorThrown, 'error');
        }
    });
}

function runScript(event) {
    if (event.which === 13 || event.keyCode === 13) {
        searchGenel('searchResult');        
    }
    return true;
}

function searchGenel(resultDiv) {
    var txtSearch = $("#txtSearchGenel").val();

    var divName = "#" + resultDiv;

    $.ajax({
        url: RootURL + 'Home/SearchGenel',
        type: 'POST',
        data: { searchSentence: txtSearch },
        async: false,
        success: function (text, data) {
            $(divName).html(text);
            $(divName).slideDown();
        },
        error: function (jqXhr, textStatus, errorThrown) {

            Message(errorThrown, 'error');
        }
    });
}

function DeleteRelationWordSentence(UN) {
    $.ajax({
        url: RootURL + 'Sentence/DeleteRelationSentenceAndWord',
        type: 'POST',
        data: { UN: UN },
        async: false,
        success: function (text, data) {
            $('#divSentenceGridResult').html(text);
            $('#rowSentence_' + UN).remove();
        },
        error: function (jqXhr, textStatus, errorThrown) {

            Message(errorThrown, 'error');
        }
    });
}

function WriteEnglish() {
    
    $.ajax({
        url: RootURL + 'StudyScreen/WriteEnglish',
        type: 'POST',
        async: false,
        success: function (text, data) {
            $('#contentIngilizcesiniYaz').html(text);
        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, 'error');
        }
    });
}

function SentenceGrupChanged() {
    //grubun listesini al

    var e = document.getElementById("EWSList");
    var UN = e.options[e.selectedIndex].value;

    if (UN == '')
        UN = EmptyGUID;

    $.ajax({
        url: RootURL + 'Sentence/GetGroupSentences',
        type: 'POST',
        async: false,
        data: { UN: UN },
        success: function (text, data) {
            $("#sentenceResultDiv").html(text);
        },
        error: function (jqXhr, textStatus, errorThrown) {
            Message(errorThrown, 'error');
        }
    });
}