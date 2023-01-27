$('.level-selector').on('change', function (e) {
    var level = $(e.target);
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
            var select = $('#Lesson');

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

$('#getLectorsButton').on('click', function () {
    var krajInput = $('#Kraj');
    var kraj = krajInput.val();

    var okresInput = $('#Okres');
    var okresy = okresInput.val();

    var levelInput = $('#Level');
    var level = levelInput.val();

    var lessonInput = $('#Lesson');
    var lesson = lessonInput.val();

    $.ajax({
        url: '/Umbraco/Api/MemberApi/GetAvailableLectors',
        type: 'GET',
        cache: false,
        data: {
            kraj: kraj,
            okresy: okresy,
            lesson: lesson,
            level: level
        },
        success: function (data) {
            var result = JSON.parse(data.Data);

            var lectorsContainer = $("#taurus-availableLectors-container");
            var lectorsWrapper = $('<div>').addClass('taurus-availableLectors-wrapper');

            if (result.length === 0) {
                $('<div>')
                    .addClass('taurus-displayLector-noneLectors')
                    .text('Ve vámi vybrané oblasti nejsou žádní lektoři.')
                    .appendTo(lectorsContainer);

                lectorsWrapper
                    .addClass('lector-show')
                    .addClass('animated');

                lectorsContainer.scrollIntoView({ behavior: 'smooth', block: 'start' });

                var title = $('#taurus-availableLectors-title');
                title.addClass('visibility-show');

                return;
            }

            // pokud dostanu nějaký lektory, je potřeba je vypsat
            for (i = 0; i < result.length; i++) {
                var obj = result[i];

                var element = $('<div>')
                    .addClass('taurus-displayLector-container');

                $('<span>')
                    .addClass('taurus-displayLector-span')
                    .text('Jméno:')
                    .appendTo(element);
                $('<div>')
                    .addClass('taurus-displayLector-name')
                    .text(obj.Name)
                    .appendTo(element);

                // tlačítka
                var buttonsContainer = $('<div>')
                    .addClass('taurus-displayLector-morebuttons');
                $('<input>')
                    .attr('type', 'button')
                    .addClass('taurus-displayLector-button')
                    .attr('id', 'taurus-contanctlector-button-' + obj.Id)
                    .val('Kontaktovat')
                    .click({ param1: obj.Id }, displayContactFormToLector)
                    .appendTo(buttonsContainer);

                $('<input>')
                    .attr('type', 'button')
                    .addClass('taurus-displayLector-button')
                    .attr('id', 'taurus-displaylector-button-' + obj.Id)
                    .val('Zobrazit profil')
                    .click({ param1: obj.Id }, displayContactFormToLector)
                    .appendTo(buttonsContainer);
                $('<div>')
                    .addClass('taurus-displayLector-ProfileContainer')
                    .attr('id', 'taurus-displayLector-ProfileContainer-' + obj.Id)
                    .appendTo(element);

                $('<span>')
                    .addClass('taurus-displayLector-span')
                    .text('Kraj doučování:')
                    .appendTo(element);

                $('<div>')
                    .addClass('taurus-displayLector-workPlace')
                    .text(obj.Kraj)
                    .appendTo(element);
                if (obj.Okresy !== null) {
                    $('<span>')
                        .addClass('taurus-displayLector-span')
                        .text('Okresy doučování:')
                        .appendTo(element);
                    $('<div>')
                        .addClass('taurus-displayLector-workPlace')
                        .text(obj.Okresy)
                        .appendTo(element);
                }

                $('<span>')
                    .addClass('taurus-displayLector-span')
                    .text('Úroveň a předměty doučování:')
                    .appendTo(element);

                var levelList = $('<ul>')
                    .addClass('taurus-displayLector-levelOfStudy');

                $.each(obj.Levels, function (i, val) {
                    var level = $('<li>')
                        .text(val.Name);
                    if (val.Lessons !== null) {
                        var lessonsList = $('<ul>')
                            .addClass('taurus-displayLector-levelOfStudy');

                        $.each(val.Lessons, function (j, val2) {
                            $('<li>')
                                .text(val2.Name)
                                .appendTo(lessonsList)
                        })

                        lessonsList.appendTo(level)
                    }
                    level.appendTo(levelList);
                });

                levelList.appendTo(element);
                element.appendTo(lectorsWrapper);
                
            }

            lectorsWrapper.appendTo(lectorsContainer);
            lectorsWrapper.addClass('lector-show');
            lectorsWrapper.addClass('animated');
            
            lectorsContainer.scrollIntoView({ behavior: "smooth", block: "start" });

            $('#taurus-availableLectors-title').addClass('visibility-show');
        }
    })

    $.getJSON('/Umbraco/Api/MemberApi/GetAvailableLectors?workArea=' + workArea + '&lesson=' + lesson + '&level=' + level, function (result) {

        var lectorsContainer = document.getElementById("taurus-availableLectors-container");
        var lectorsWrapper = document.createElement("div");
        lectorsWrapper.classList.add("taurus-availableLectors-wrapper");

        lectorsContainer.textContent = '';
        var input = JSON.parse(result);

        if (input.length === 0) {
            var noneLectors = document.createElement("div");
            noneLectors.classList.add("taurus-displayLector-noneLectors");
            noneLectors.innerHTML = "Ve vámi vybrané oblasti nejsou žádní lektoři.";

            lectorsContainer.appendChild(noneLectors);
            lectorsWrapper.classList.add("lector-show");
            lectorsWrapper.classList.add("animated");

            lectorsContainer.scrollIntoView({ behavior: "smooth", block: "start" });

            var title = document.getElementById("taurus-availableLectors-title");
            title.classList.add("visibility-show");

            return;
        }

        for (i = 0; i < input.length; i++) {
            var obj = input[i];

            var element = document.createElement("div");
            element.classList.add("taurus-displayLector-container");

            var spanName = document.createElement("span");
            spanName.classList.add("taurus-displayLector-span");
            spanName.innerHTML = "Jméno:";

            var name = document.createElement("div");
            name.classList.add("taurus-displayLector-name");
            name.innerHTML = obj.Name;

            var buttonsContainer = document.createElement("div");
            buttonsContainer.classList.add("taurus-displayLector-morebuttons");

            var callingButton = document.createElement("input");
            callingButton.type = 'button';
            callingButton.classList.add("taurus-displayLector-button");
            callingButton.id = "taurus-contanctlector-button-" + obj.Id;
            callingButton.value = "Kontaktovat";

            var profileDetails = document.createElement("div");
            profileDetails.classList.add("taurus-displayLector-ProfileContainer");
            profileDetails.id = "taurus-displayLector-ProfileContainer-" + obj.Id;

            var displayProfil = document.createElement("input");
            displayProfil.type = 'button';
            displayProfil.classList.add("taurus-displayLector-button");
            displayProfil.id = "taurus-displaylector-button-" + obj.Id;
            displayProfil.value = "Zobrazit profil";

            $(displayProfil).click({ param1: obj.Id }, displayLectorProfile);
            $(callingButton).click({ param1: obj.Id }, displayContactFormToLector);

            buttonsContainer.appendChild(callingButton);
            buttonsContainer.appendChild(displayProfil);

            var spanPlace = document.createElement("span");
            spanPlace.classList.add("taurus-displayLector-span");
            spanPlace.innerHTML = "Místo doučování:";

            var workPlace = document.createElement("div");
            workPlace.classList.add("taurus-displayLector-workPlace");
            workPlace.innerHTML = obj.WorkPlace;

            var spanLevel = document.createElement("span");
            spanLevel.classList.add("taurus-displayLector-span");
            spanLevel.innerHTML = "Úroveň doučování:";

            var levelOfStudy = document.createElement("ul");
            levelOfStudy.classList.add("taurus-displayLector-levelOfStudy");
            var level = null;
            if (obj.LevelOfStudy.elementarySchool === true) {
                level = document.createElement("li");
                level.innerHTML = "Základní škola";
                levelOfStudy.appendChild(level);
            }
            if (obj.LevelOfStudy.highSchool === true) {
                level = document.createElement("li");
                level.innerHTML = "Střední škola";
                levelOfStudy.appendChild(level);
            }
            if (obj.LevelOfStudy.college === true) {
                level = document.createElement("li");
                level.innerHTML = "Vysoká škola";
                levelOfStudy.appendChild(level);
            }

            element.appendChild(spanName);
            element.appendChild(name);
            element.appendChild(spanPlace);
            element.appendChild(workPlace);
            element.appendChild(spanLevel);
            element.appendChild(levelOfStudy);
            element.appendChild(buttonsContainer);
            element.appendChild(profileDetails);

            lectorsWrapper.appendChild(element);
        }

        lectorsContainer.appendChild(lectorsWrapper);
        lectorsWrapper.classList.add("lector-show");
        lectorsWrapper.classList.add("animated");

        lectorsContainer.scrollIntoView({ behavior: "smooth", block: "start" });

        var title = document.getElementById("taurus-availableLectors-title");
        title.classList.add("visibility-show");

    });
})