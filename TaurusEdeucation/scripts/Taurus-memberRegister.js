$(document).ready(function() {
    var error = $('#lektor-registration-error');
    if (error.length === 0) {
        return;
    }

    var success = error.attr('data-success')
    if (success === 'True') {
        var modalDiv = document.getElementById("modal-content");

        conditionsDiv.innerHTML = '';

        var modal = document.getElementById("modal-window");

        modal.style.display = "block";
        return
    }
    else {
        showMessage('Pri registraci se vyskytla chyba.', true);
    }

    var form = document.getElementById('Taurus-NewLectorForm');
    
    form.style.display = 'block';
    form.scrollIntoView({behavior: "smooth", block: "end" });
    
    var krajInput = $('#Kraj');
    var kraj = krajInput.val();
    var okresInput = $('#Okres');
    var okresy = okresInput.val();
    if (kraj !== '') {
        setMap(kraj);
    }

    var array = okresy.split(';');
    $.each(array, function(i, val) {
        if (val !== '') {
            var okres = $('#' + val);
            if (okres.length !== 0) {
                okres.attr('data-selected', true).addClass('okres-active');
            }   
        }
    }); 
});

$('.level-selector').on('change', function (e) {
    var level = $(e.target);
    var levelId = level.attr('data-order');
    var levelVal = level.val();

    $.ajax({
        url: '/Umbraco/api/MemberApi/GetLessonsSelect',
        type: 'GET',
        cache: false,
        data: {
            levelId: levelVal
        },
        success: function (data) {
            var lessons = JSON.parse(data.Data);
            var select = $('#Lessons_' + levelId + '_');

            select.empty();
            select.attr('style', 'visibility: visible');

            $('<option>')
                .attr('disabled', 'disabled')
                .attr('selected', 'selected')
                .attr('val', '')
                .text('Vyberte možnost')
                .prependTo(select);

            $.each(lessons, function (i, val) {
                $('<option>')
                    .attr('value', val.Id)
                    .text(val.Name)
                    .appendTo(select);
            });
        }
    });
});