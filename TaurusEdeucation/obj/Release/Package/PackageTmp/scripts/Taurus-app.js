// funkce pro ukázání možností vybrat další lekci
function NextLesson(lastSelect) {
    if (lastSelect.changed) {
        return;
    }

    lastSelect.changed = true;
    var nextLesson = document.getElementById(parseInt(lastSelect.id)+1);
    
    nextLesson.style = "visibility:visible";
}

function checkLessons() {
    var elements = document.getElementsByClassName("taurus-newlector-dropdown");
    
    Array.prototype.forEach.call(elements, function(item) {
        if (item.selectedIndex !== 0) {
            item.changed = true;
            item.style.display = "block";
            
            var nextLesson = document.getElementsByName("lessons[" + item.id + "]")[0];
            if (nextLesson.selectedIndex !== 0) {
                nextLesson.style.display = "block";
            }
            if (nextLesson.selectedIndex === 0) {
                nextLesson.style.display = "block";
                return;
            }
        }
        else {
            return;
        }
    });
}

function sendLectorWorkPlace() {
    var lectorId = document.getElementById("taurus-newLectorId").value;
    var workArea = document.getElementById("taurus-czech-map-selected").value;
    var workPlace = document.getElementById("taurus-map-additionalplace").value;
    
    $.ajax({
        url: '/Umbraco/Api/MemberApi/UpdateMemberWorkPlace',
        type: 'POST',
        cache: false,
        data: { 
            memberId: lectorId, 
            workArea: workArea,
            workPlace: workPlace },
   });
   
   var modal = document.getElementById("myModalMap");
   modal.style.display = "none";
   
   var modalMessage = document.getElementById("myModalMessage");
   modalMessage.style.display = "block";
}

function getAvailableLectors() {
    var workArea = document.getElementById("taurus-czech-map-selected").value;
    var lesson = document.getElementById("lesson").value;
    var level = document.getElementById("level").value;
    
    $.getJSON('/Umbraco/Api/MemberApi/GetAvailableLectors?workArea=' + workArea + '&lesson=' + lesson + '&level=' + level, function(result) {
        
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
        
            lectorsContainer.scrollIntoView({behavior: "smooth", block: "start"});
            
            var title = document.getElementById("taurus-availableLectors-title");
            title.classList.add("visibility-show");
            
            return;
        }
        
        for (i = 0; i< input.length; i++) {
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
            
            $(displayProfil).click( {param1: obj.Id}, displayLectorProfile );
            $(callingButton).click( {param1: obj.Id}, displayContactFormToLector );
            
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
        
        lectorsContainer.scrollIntoView({behavior: "smooth", block: "start"});
        
        var title = document.getElementById("taurus-availableLectors-title");
        title.classList.add("visibility-show");
        
    });
}

function displayLectorProfile(event) {
    $.getJSON('/Umbraco/Api/MemberApi/GetLectorProfile?id=' + event.data.param1, function(result) {
        
        var profileDetails = document.getElementById("taurus-displayLector-ProfileContainer-" + event.data.param1);
        profileDetails.classList.remove("lector-show");
        profileDetails.classList.remove("animated");
        profileDetails.textContent = '';
        
        var profileContainer = document.createElement("div");
        profileContainer.classList.add("taurus-displayLector-ProfileWrapper");
        
        var profileImage = document.createElement("img");
        profileImage.classList.add("taurus-displayLector-profilePicture");
        profileImage.src = result.ThumbnailImagePath;
        
        var spanFirstName = document.createElement("span");
        spanFirstName.classList.add("taurus-displayLector-Profile-span");
        spanFirstName.innerHTML = "Jméno:";
        
        var firstName = document.createElement("div");
        firstName.classList.add("taurus-displayLector-Profile-FirstName");
        firstName.innerHTML = result.FirstName;
        
        var spanLastName = document.createElement("span");
        spanLastName.classList.add("taurus-displayLector-Profile-span");
        spanLastName.innerHTML = "Příjmení:";
        
        var lastName = document.createElement("div");
        lastName.classList.add("taurus-displayLector-Profile-LastName");
        lastName.innerHTML = result.LastName;
        
        var spanWorkPlace = document.createElement("span");
        spanWorkPlace.classList.add("taurus-displayLector-Profile-span");
        spanWorkPlace.innerHTML = "Místo doučování";
        
        var workPlace = document.createElement("div");
        workPlace.classList.add("taurus-displayLector-Profile-workPlace");
        workPlace.innerHTML = result.WorkPlace;
        
        var spanResume = document.createElement("span");
        spanResume.classList.add("taurus-displayLector-Profile-span");
        spanResume.innerHTML = "Životopis";
        
        var resume = document.createElement("div");
        resume.classList.add("taurus-displayLector-Profile-resume");
        resume.innerHTML = result.Resume;
        
        var spanLessons = document.createElement("span");
        spanLessons.classList.add("taurus-displayLector-Profile-span");
        spanLessons.innerHTML = "Vyučované předměty:";
        
        var lessons = document.createElement("ul");
        lessons.classList.add("taurus-displayLector-Profile-Lessons");
        
        for (j = 0; j < result.Lessons.length; j++)
        {
            var lesson = document.createElement("li");
            lesson.innerHTML = result.Lessons[j];
            
            lessons.appendChild(lesson);
        }
        
        var spanLevels = document.createElement("span");
        spanLevels.classList.add("taurus-displayLector-Profile-span");
        spanLevels.innerHTML = "Úroveň doučování";
        
        var levels = document.createElement("ul");
        levels.classList.add("taurus-display-Lector-Profile-Levels");
        
        var lvl = null;
        
        if (result.Elementary) {
            lvl = document.createElement("li");
            lvl.innerHTML = "Základní škola";
            levels.appendChild(lvl);
        }
        if (result.High) {
            lvl = document.createElement("li");
            lvl.innerHTML = "Střední škola";
            levels.appendChild(lvl);
        }
        if (result.College) {
            lvl = document.createElement("li");
            lvl.innerHTML = "Vysoká škola";
            levels.appendChild(lvl);
        }
        
        profileContainer.appendChild(profileImage);
        profileContainer.appendChild(spanFirstName);
        profileContainer.appendChild(firstName);
        profileContainer.appendChild(spanLastName);
        profileContainer.appendChild(lastName);
        profileContainer.appendChild(spanWorkPlace);
        profileContainer.appendChild(workPlace);
        profileContainer.appendChild(spanResume);
        profileContainer.appendChild(resume);
        profileContainer.appendChild(spanLessons);
        profileContainer.appendChild(lessons);
        profileContainer.appendChild(spanLevels);
        profileContainer.appendChild(levels);
        
        
        profileDetails.appendChild(profileContainer);
        
        profileDetails.classList.add("lector-show");
        profileDetails.classList.add("animated");
        
        profileDetails.scrollIntoView({behavior: "smooth", block: "start"});
    });
}

function displayContactFormToLector(event) {
    var modal = document.getElementById("myModalForm");
    modal.style.display = "block";
    
    var idinput = document.getElementById("lectorId");
    idinput.value = event.data.param1;
}

function sendMessageToLector() {
    var idElement = document.getElementById("lectorId");
    var id = idElement.value;
    
    var message = document.getElementById("message").value;
    var firstName = document.getElementById("firstName").value;
    var lastName = document.getElementById("surName").value;
    var email = document.getElementById("email").value;
    var phone = document.getElementById("phone").value;
    var levelOfStudy = document.getElementById("level").value;
    var lesson = document.getElementById("lesson").value;
    var locationInput = document.getElementById("locationDropDown").value;
    
    if (locationInput == "atLectors")
    {
        var location = "U Vás";
    }
    else
    {
        var location = "U studenta\n" + document.getElementById("studentStreet").value + ", " + document.getElementById("studentCity").value;
    }
    $.ajax({
        url: '/Umbraco/Api/MemberApi/SendMessageToLector',
        type: 'POST',
        cache: false,
        data: { 
            Id: id, 
            Message: message,
            StudentName: firstName,
            StudentLastName: lastName,
            StudentEmail: email,
            StudentPhone: phone,
            LevelsOfStudy: levelOfStudy,
            Lesson: lesson,
            Location: location
            },
   });
   
   modal = document.getElementById("myModalForm");
   modal.style.display = "none";
}

function DisplayStudentAddressInput(){
    var option = document.getElementById("locationDropDown");
    
    if (option.value == "atLectors")
    {
        document.getElementById("studentCityLabel").style = "display: none";
        document.getElementById("studentCity").style = "display: none";
        document.getElementById("studentStreetLabel").style = "display: none";
        document.getElementById("studentStreet").style = "display: none";
    }
    else
    {
        document.getElementById("studentCityLabel").style = "vibility:visible";
        document.getElementById("studentCity").style = "vibility:visible";
        document.getElementById("studentStreetLabel").style = "vibility:visible";
        document.getElementById("studentStreet").style = "vibility:visible";
    }
}

function selectArea(selectObject) {
    var selectedValue = selectObject.value;
    
    var textInput = document.getElementById('taurus-czech-map-selected');
    textInput.value = selectedValue;
    
    var form = document.getElementById('Taurus-StudentForm');
    form.style.display = 'block';
    
    form.scrollIntoView({behavior: "smooth", block: "end" }); 
}

function selectNewLectorRegion(selectObject) {
    var selectedValue = selectObject.value;
    
    var textInput = document.getElementById('taurus-czech-map-selected');
    textInput.value = selectedValue;
    
    var form = document.getElementById('taurus-map-additionalplace-container');
    form.style.display = 'block';
}

function downloadConditions() {
    window.location = '/Umbraco/Api/conditions/SaveConditionsAsPdf';
}

function SaveNewStudent(){
    var name = document.getElementById("newStudentName").value;
    var lesson = document.getElementById("newStudentLesson").value;
    var variables = "?name=" + name + "&lesson=" + lesson;
    
    $.ajax({
        url: '/Umbraco/api/OverviewApi/SaveStudent'+variables,
        type: 'POST',
    });

    location.reload();
}