window.onkeypress = async function (e) {
    e = e || window.event;
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var button = document.querySelector('.mud-button[name="tryEnter"]')
        if(button != null)
            button.click();
    }
};