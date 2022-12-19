window.onscroll = function () { onScroll() };
function onScroll() {
    if (document.body.scrollTop > 50 || document.documentElement.scrollTop > 50) {
        document.getElementById("img-shrink").classList.add("sticky");
        document.getElementById("bnr").classList.add("increase");
        document.getElementById("main_nav99").classList.add("removeCurrency");
    } else {
        document.getElementById("img-shrink").classList.remove("sticky");
        document.getElementById("bnr").classList.remove("increase");
        document.getElementById("main_nav99").classList.remove("removeCurrency");

    }
}

$("#hamburger-button").on("click", function () {
    $('#menu-hamburger-label ul').css('display', 'block');
    $('#menu-hamburger-label ul').css('z-index', '200');
});