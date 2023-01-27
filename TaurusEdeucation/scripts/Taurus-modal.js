function showMessageForLector() {
    var modal = document.getElementById("myModalForm");
    
    modal.style.display = "block";
}

// funkce pro zavření modálního okna
$('#modal-close').on('click', function () {
    closeModal();
});

// funkce pro zavření modální okna přes kliknutí mimo něj
window.onclick = function(event) {    
    modal = document.getElementById("modal-window");
    
    if (event.target == modal) {
        modal.style.display = "none";
    }
};

function closeModal() {
    var modal = document.getElementById('modal-window');

    modal.style.display = "none";
}

function redirectLogin() {
    location.replace("/login");
}