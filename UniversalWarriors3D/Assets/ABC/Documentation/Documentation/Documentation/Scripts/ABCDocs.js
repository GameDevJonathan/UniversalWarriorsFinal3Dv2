
//************* GLOBAL  ********************************************


function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, '\\$&');
    var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, ' '));
}


function HyperLink(Config, Tab, Header, Setting) {

    var url = window.location.href.split("?")[0];

    //If local reconstruct url using starting part
    if (url.toLowerCase().indexOf("file:/") >= 0) {
        var count = window.location.href.split("/").length

        var urlsplit = window.location.href.split("/")
        urlsplit.pop();
        urlsplit.pop();
       
        var url = urlsplit.join("/") + "/Documentation.html";

    } else {
        // else if online just take from top
        url = window.top.location.href.split("?")[0];
    }

    url = url + "?Config=" + Config + "&Tab=" + Tab + "&Header=" + Header + "&Setting=" + Setting;

    window.top.location.href = url;
    
}


function ContentJump() {

    //example:
    //http://localhost:50182/Docs/?Config=ControllerConfig&Tab=Settings&Header=HeaderMana&Setting=Mana_GUI

    var configVal = getParameterByName('Config');
    var tabVal = getParameterByName('Tab');
    ;
    //select config dropdown
    if (configVal !== null) {
        configVal = configVal.replace("AND", "&");

        $("#ConfigSelector")
            .val(configVal)
            .trigger('change');


        //click tab button at top
        if (tabVal !== null) {
            tabVal = tabVal.replace("AND", "&");

            $('#' + configVal + ' p')
                .filter(function () { return $(this).text() === tabVal; })
                .click();

            $('#' + configVal + ' select')
                .val(tabVal);


            $('#' + configVal + ' option').filter(function () {
                return this.text === tabVal;
            }).attr('selected', true);
                
        }
    }

}



//************* CONFIG CONTAINER (Documentation.html) PAGE  ********************************************

//Will load config (Ability Manager/ABC Controller/Statemanager etc). Loads the top nav bar with links that once selected will load the correct Iframe
//Each Iframe is a tab of settings (top row in config screen)
function LoadConfig(id) {

    $(".Config").each(function (index) {
        $(this).addClass("hidden");
    });

    $("#" + id).removeClass("hidden");

    //if no config provided in parameters then default to the first in dropdown
    var configVal = getParameterByName('Config');

    if (configVal === null) {
        $("#" + id + " p:first").click();
    }

}

//will load the content pagess in the iframe
function loadIframe(iframeName, url, element) {

    $(".header-title p").each(function (index) {
        $(this).removeClass("header-title-active");
    });

    if (element != null) {
        $(element).addClass("header-title-active");
    }

    var parameters = "?" + window.location.href.split("?")[1];



    $iframe = $('#' + iframeName).attr('src', url+parameters);
}


//************* CONTENT PAGES (content pages are loaded dynamically from static html pages)  ********************************************

//Will show hide divs, depending on the navigation list to the left selected (Each navigation link is a different setting section (tabs to left)
function LoadContent(id) {

    $(".Section").each(function (index) {
        $(this).addClass("hidden");
    });

    sectionId = id.replace("&", "\\&");
    $("#" + sectionId).removeClass("hidden");

}


//Will generate the navigation list by reading the H2 and H3 tags
function GenerateNavigationList() {
    //Cycle through each Setting Section (H2 Headers)
    $("h2").each(function (index) {

        var h2Title = $(this).text();
        $("#Main-Sidebar-Nav").append('<lh> <label class="tree-toggler nav-header sidebar-title">' + h2Title + '</label><ul class="nav nav-list tree" id="Header' + h2Title.replace(/ /g, "_") + '"></ul>');


        //Cycle through each Setting (H3 Headers)
        $("#" + h2Title.replace(/ /g, "_").replace("&", "\\&") + " > h3").each(function (index) {
            var h3Title = $(this).text();
            $("#Header" + h2Title.replace(/ /g, "_").replace("&", "\\&")).append('<li onClick="LoadContent(\'' + h2Title.replace(/ /g, "_").replace("&", "\\&") + '\')"><a class="silver-text" href="#' + h3Title.replace(/ /g, "_") + '">' + h3Title + '</a></lh>');
        });
    });


    $('label.tree-toggler').click(function () {
        $(this).parent().children('ul.tree').toggle(300);
    });


    //Content jump to correct spot if in parameters (auto clicks setting to side and scrolls to setting)
    var headerVal = getParameterByName('Header');
    var settingVal = getParameterByName('Setting');

    if (headerVal !== null) {
        headerVal = headerVal.replace("AND", "\\&");


        $('#Main-Sidebar').find('#' + headerVal + ' a').first().click();

        if (settingVal !== null) {
            settingVal = settingVal.replace("AND", "\\&");


            $('#Main-Content').scrollTop($('#Main-Content').find("#" + settingVal).offset().top);
        }
    }

}