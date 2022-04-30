function setTheme(themeName) {
    localStorage.setItem('theme', themeName);
    document.documentElement.className = themeName;
}

// function to toggle between light and dark theme
function toggleTheme() {
    if (localStorage.getItem('theme') === 'theme-dark') {
        setTheme('theme-light');
        $('.logo-light').css('display', 'block');
        $('.logo-dark').css('display', 'none');
        $('#hamburgerMenu').css('display', 'none');
        $('#hamburgerLight').css('display', 'block');
    } else {
        setTheme('theme-dark');
        $('.logo-light').css('display', 'none');
        $('.logo-dark').css('display', 'block');
        $('#hamburgerMenu').css('display', 'block');
        $('#hamburgerLight').css('display', 'none');
    }
}

// Immediately invoked function to set the theme on initial load
(function () {
    if (localStorage.getItem('theme') === 'theme-dark') {
        setTheme('theme-dark');

        $('.logo-light').css('display', 'none');
        $('.logo-dark').css('display', 'block');

        document.getElementById('slider').checked = false;

        //document.getElementsByClassName('logo-light').style.display = "none";
        //document.getElementsByClassName('logo-dark').style.display = "block";
    } else {
        setTheme('theme-light');

        $('.logo-light').css('display', 'block');
        $('.logo-dark').css('display', 'none');

        document.getElementById('slider').checked = true;
    }
})();