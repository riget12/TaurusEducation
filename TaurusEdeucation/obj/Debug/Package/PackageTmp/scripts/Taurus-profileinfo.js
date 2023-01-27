$(document).ready(() => {
    $.ajax({
        url: '/Umbraco/Api/MemberProfileEditApi/CheckPoznamka',
        type: 'GET',
        cache: false,
        success: function (result) {
            if (result === null) {
                return;
            }

            var data = JSON.parse(result.Data);

            var contentDiv = document.getElementById('modal-content')
            contentDiv.innerHTML = data;
            var modal = document.getElementById('modal-window');
            modal.style.display = 'block';
        }
    });
});

$('#mdb_ok').on('click', function () {
    $.ajax({
        url: '/Umbraco/Api/MemberProfileEditApi/ReadPoznamka',
        type: 'POST',
        cache: false,
        success: function () {
            closeModal();
        }
    });
});
                        