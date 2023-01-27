$(document).ready(function () {
    $.ajax({
        url: '/Umbraco/Api/MemberProfileEditApi/PopulateProfileModel',
        type: 'POST',
        cache: false,
        success: function(result){
            document.getElementById("firstName").value = result.firstName;
            document.getElementById("surName").value = result.surName;
            document.getElementById("city").value = result.city;
            document.getElementById("street").value = result.street;
            document.getElementById("phone").value = result.phone;
            document.getElementById("email").value = result.email;
            document.getElementById('lector_id').value = result.id;
            document.getElementById("elementary").checked = result.elementarySchool;
            document.getElementById("high").checked = result.highSchool;
            document.getElementById("college").checked = result.college;
            
            for (var i = 0; i < 10; i++)
            {
                var lesson = result.lessons[i];
                var lessonDropDown = document.getElementById(i+1);
                
                if (lesson === "")
                {
                    if (i > 0)
                    {
                        if (document.getElementById(i).value != "Vyber")
                        {
                            lessonDropDown.style = "visibility : visible";
                        }
                        else
                        {
                            lessonDropDown.style = "display : none";
                        }
                    }
                    else
                    {
                        lessonDropDown.style = "visibility : visible";
                    }
                }
                else
                {
                    lessonDropDown.value = lesson;
                    lessonDropDown.remove(0);
                    lessonDropDown.style = "visibility : visible";
                }
            }
            
            setProfileImage();
            setThumbnailImage();
        }
   });
});

$(document).ready(function() {
   if (window.location.pathname === "/registeredlector/profile/") {
       var element = document.getElementById('lectorId');
       
       $.ajax({
          url: '/Umbraco/api/MemberProfileEditApi/GetThumbnailImage?lectorId=' + element.value,
          cache: false,
          contentType: false,
          processData: false,
          async: false,
          success: function(response) {
            $('#thumbnailImage').attr('src', response);
          }
       });
   }
});

$('#upload').on('click', function () {
    var file_data = $('#file').prop('files')[0];
    var form_data = new FormData();
    
    var input = document.getElementById('lector_id');
    
    form_data.append('file', file_data);
    form_data.append('lectorId', input.value);
    $.ajax({
        url: '/Umbraco/api/MemberProfileEditApi/Create',
        cache: false,
        contentType: false,
        processData: false,
        async: false,
        data: form_data,
        type: 'POST',
        success: function (response) {

            var data = JSON.parse(response.Data);
            
            var element2 = document.getElementById('thumbnail-profile-image');
            element2.src = 'data:image/' + data.Extension + ';base64,' + data.ImageData;
            element2.name = data.ImageName;
        },
        error: function (response) {
            
        }
    });
});

function setProfileImage() {
    var input = document.getElementById('lector_id');
    
    $.ajax({
       url: '/Umbraco/api/MemberProfileEditApi/GetProfileImage?lectorId=' + input.value,
       type: 'GET',
       cache: false,
       async: false,
       success: function(response) {
           $('#profile-image').attr( 'src', response);
       }
    });
}

function setThumbnailImage() {
    var input = document.getElementById('lector_id');
    
    $.ajax({
        url: '/Umbraco/api/MemberProfileEditApi/GetThumbnailImage?lectorId=' + input.value,
        type: 'GET',
        cache: false,
        async: false,
        success: function(response) {
            $('#thumbnail-profile-image').attr( 'src', response);
        }
    });
}

$('#submit-profile-image').on('click', function() {
    
    element = document.getElementById('thumbnail-profile-image');
    var thumbnailSrc = element.src;
    var imageName = element.name;
    
    element = document.getElementById('lector_id');
    var lectorId = element.value;
    
    $.ajax({
       url: '/Umbraco/api/MemberProfileEditApi/SaveNewProfileImage',
       type: 'POST',
       cache: false,
       async: false,
       data: { 
           imageName: imageName,
           thumbnailProfileImage: thumbnailSrc,
           lectorId: lectorId
       },
       success: function(response) {
           document.location.href = document.referrer;
       }
    });
});

function UpdateCalendar() {
    
    var year = document.getElementById("yearControl").value;
    var month = document.getElementById("monthControl").value;
    var variables = "?year=" + year + "&month=" + month;

    $.ajax({
        type: "POST",
        url: "/umbraco/api/OverviewApi/GetDaysInMonth" + variables,
        success: function(response) {
            for (var i = 29; i <= response; i++)
            {
                document.getElementById("name"+(i)).style = "visibility: visible";
                document.getElementById("hour"+(i)).style = "visibility: visible";
                document.getElementById("price"+(i)).style = "visibility: visible";
            }
            for (var j = response+1; j <= 31; j++)
            {
                document.getElementById("name"+(j)).style = "visibility: visible";
                document.getElementById("hour"+(j)).style = "display: none";
                document.getElementById("price"+(j)).style = "display: none";
            }
        }
    });
}

function UpdateData() {
    
    var name = document.getElementById("studentName").value;
    var year = document.getElementById("yearControl").value;
    var month = document.getElementById("monthControl").value;
    var variables = "?name=" + name + "&year=" + year + "&month=" + month;
    
    $.ajax({
        type: "POST",
        url: "/umbraco/api/OverviewApi/GetData" + variables,
        success: function(response) {
            for (var i = 0; i < 31; i++)
            {
                document.getElementById("hour"+(1+i)).value = response[i].hour;
                document.getElementById("price"+(1+i)).value = response[i].price;
            }
        }
    });
}

function YearControl(){
    var yearControl = document.getElementById("yearControl");
    var year = yearControl.value;
    
    if (year < 2020)
    {
        yearControl.value = 2020;
    }
    if (year > 2100)
    {
        yearControl.value = 2100;
    }
    
    UpdateCalendar();
    UpdateData();
}

function MonthControl() {
    var studentControl = document.getElementById("studentName");
    var yearControl = document.getElementById("yearControl");
    var monthControl = document.getElementById("monthControl");
    var month = monthControl.value;
    
    if (month == 1 && monthControl.last == 1 && yearControl.value > 2020)
    {
        yearControl.value = yearControl.value - 1;
        monthControl.value = 12;
    }
    if (month == 12 && monthControl.last == 12 && yearControl.value < 2100)
    {
        yearControl.value = parseInt(yearControl.value) + 1;
        monthControl.value = 1;
    }
    monthControl.last = monthControl.value;
}

function MonthCheck() {
    var monthControl = document.getElementById("monthControl");
    var month = monthControl.value;
    
    if (month > 12)
    {
        monthControl.value = 12;
        monthControl.last = 12;
    }
    if (month < 1)
    {
        monthControl.value = 1;
        monthControl.last = 1;
    }
    
    UpdateCalendar();
    UpdateData();
}