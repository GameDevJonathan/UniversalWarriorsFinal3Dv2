

//************* GENERATE DYNAMIC MODAL  ********************************************

//      <!--Dynamic Media Modal -- >
//<div class="modal fade" id="DynamicMediaModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
//    <div class="modal-dialog modal-lg " role="document">
//        <div class="modal-content">
//            <div id="modal-body" class="modal-body">
//                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
//                    <span aria-hidden="true">&times;</span>
//                </button>
//                <video id="DynamicSourceVideo" class="image-border-thick-groove" controls autoplay muted="muted">
//                    <source src="" type="video/mp4">
//                        Your browser does not support the video.
//                </video>
//                    <img id="DynamicSourceImage" src="" width="360" height="230" class="fluid-img image-border" />
//            </div>
//            </div>
//        </div>
//    </div>




$(document).ready(function () {

    var DynamicModalHtml = '<div class="modal fade" id="DynamicMediaModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">' +
        '    <div class="modal-dialog modal-lg " role="document">' +
        '        <div class="modal-content">' +
        '            <div id="modal-body" class="modal-body">' +
        '                <button type="button" class="close" data-dismiss="modal" aria-label="Close">' +
        '                    <span aria-hidden="true">×</span>' +
        '                </button>' +
        '                <video id="DynamicSourceVideo" class="image-border-thick-groove" controls autoplay muted="muted" style="width:100%">' +
        '                    <source src="" type="video/mp4">' +
        '                    Your browser does not support the video.' +
        '                </video>' +
        '                <img id="DynamicSourceImage" src="" width="360" height="230" class="fluid-img image-border" />' +
        '            </div>' +
        '        </div>' +
        '    </div>' +
        '</div>';


    //If dynamic modal doesn't exist then add it 
    if ($("#DynamicMediaModal").length == 0) {
        $("body").append(DynamicModalHtml);
    }


});



//************* GLOBAL  ********************************************



//Will load either an image or video in a shared media modal 
function LoadDynamicMedia(type, url) {

    if (type == "video") {
        $('#DynamicSourceVideo source').attr('src', url);
        $("#DynamicSourceVideo")[0].load()
        $("#DynamicSourceVideo").show();
        $('#DynamicSourceImage').hide();
    } else if (type == "image") {
        $('#DynamicSourceImage').attr('src', url);
        $("#DynamicSourceVideo").hide();
        $('#DynamicSourceImage').show();
    }
}

//play video on hover if in desktop mode (and also only show video controls in mobile mode)
$(document).on('mouseover', '.hoverplayvideo', function (e) {
    var video = $(this).get(0);
    if ($(window).width() > 960) {
        video.removeAttribute("controls")
        const playPromise = video.play();
        if (playPromise !== null) {
            playPromise.catch(() => { video.play(); })
        }       
        return false;
    } else {
        video.setAttribute("controls", "controls")
    }
});

//pause video on mouse leave if in desktop mode (and also only show video controls in mobile mode)
$(document).on('mouseleave', '.hoverplayvideo', function (e) {
    var video = $(this).get(0);
    if ($(window).width() > 960) {
        video.removeAttribute("controls")
        video.pause();
        return false;
    } else {
        video.setAttribute("controls", "controls")
    }
});