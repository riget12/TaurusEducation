$(document).ready(function () {
    $.ajax({
        url: '/Umbraco/Api/MemberProfileEditApi/PopulateProfileModel',
        type: 'POST',
        cache: false,
        success: function(result){
            if (result === null) {
                return;
            }
            
            var firstName = $('#firstName');
            if (firstName.lenght !== 0) {
                firstName.val(result.firstName);
            }
            
            var surName = $('#surName');
            if (surName.lenght !== 0) {
                surName.val(result.surName);
            }
            
            var city = $('#city');
            if (city.lenght !== 0) {
                city.val(result.city);
            }
            
            var street = $('#street');
            if (street.lenght !== 0) {
                street.val(result.street);
            }
            
            var phone = $('phone');
            if (phone.lenght !== 0) {
                phone.val(result.phone);
            }
            
            var email = $('#email');
            if (email.lenght !== 0) {
                email.val(result.email);
            }
            
            var lectorId = $('#lector_id');
            if (lectorId.lenght !== 0) {
                lectorId.val(result.id);
            }
            
            var elementary = $('#elementary');
            if (elementary.lenght !== 0) {
                elementary.val(result.elementarySchool);
            }
            
            var high = $('#high');
            if (high.lenght !== 0) {
                high.val(result.highSchool);
            }
            
            var college = $('#colleget');
            if (college.lenght !== 0) {
                college.val(result.college);
            }
            
            for (var i = 0; i < 10; i++)
            {
                var lesson = result.lessons[i];
                var id = (i + 1);
                var lessonDropDown = $('#' + id);
                
                if (lessonDropDown.length === 0) {
                    continue;
                }
                if (lesson === "")
                {
                    if (i > 0)
                    {
                        var choiceI = $('#' + i);
                        if (choiceI.length !== 0 && choiceI.val() != "Vyber")
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

$('#lector-hide').on('click', function() {
   var lector = $('#lectorId').val();
   var checked = false;
   
   if ($(this).is(':checked')) {
        checked = true;    
   } else {
        checked = false;
   }
   
   $.ajax ({
      type: 'POST' ,
      url: '/Umbraco/api/MemberProfileEditApi/HideMember?lectorId=' + lector + '&hide=' + checked,
      //data: { lectorId: lector, hide: checked },
      cache: false,
      success: function(response) {
          showMessage('Změna viditelnosti byla provedena.');
      }
   });
});

function setProfileImage() {
    var input = $('#lector_id');
    if (input.length === 0) {
        return;
    }
    
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
    var input = $('#lector_id');
    if (input.length === 0) {
        return;
    }

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

$('#deleteLector').on('click', function () {
    $.ajax({
        url: '/Umbraco/Api/MemberProfileEditApi/DeleteLector',
        typ: 'POST',
        cache: false,
        success: function () {
            
        }
    })
});