var slideIndex = 1;
showSlides(slideIndex);
function plusSlides(n) {
    showSlides(slideIndex += n);
}
function currentSlide(n) {
    showSlides(slideIndex = n);
}
function showSlides(n) {
    var i;
    var slides = document.getElementsByClassName("mySlides");
    var dots = document.getElementsByClassName("dots");
    var buttonIndex = document.getElementsByClassName("buttonIndex");

    if (n > slides.length) {
        slideIndex = 1
    }
    if (n < 1) {
        slideIndex = slides.length
    }
    for (i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
    }
    for (i = 0; i < buttonIndex.length; i++) {
        buttonIndex[i].className = buttonIndex[i].className.replace(" btn-active-dark", "");
    }
    slides[slideIndex - 1].style.display = "block";
    buttonIndex[slideIndex - 1].className += " btn-active-dark";
    //    dots[slideIndex - 1].className += " active";
}