function showMessageForLector() {
    var modal = document.getElementById("myModalForm");
    
    modal.style.display = "block";
}

// funkce pro otevření okna s podmínkami
function viewConditions() {
    $.getJSON('/Umbraco/Api/Conditions/GetConditions', function(result) {
      var conditionsDiv = document.getElementById("conditions");
      
      conditions.innerHTML = result;
    });
    
    var modal = document.getElementById("myModal");

    modal.style.display = "block";
}

// funkce pro zavření modálního okna
function closeModal() {
    var id = event.target.id.toString();
    var modalId = id.replace("close", "");
    var modal = document.getElementById(modalId);
    modal.style.display = "none";
    
    if (modalId === "myModalMessage") {
       redirectLogin(); 
    }
}

// funkce pro zavření modální okna přes kliknutí mimo něj
window.onclick = function(event) {
    var modal = document.getElementById("myModal");
    
    if (event.target == modal) {
        modal.style.display = "none";
        return;
    }
    
    modal = document.getElementById("myModalMap");
    
    if (event.target == modal) {
        modal.style.display = "none";
    }
    
    modal = document.getElementById("myModalMessage");
    
    if (event.target == modal) {
        modal.style.display = "none";
        
        redirectLogin();
    }
    
    modal = document.getElementById("myModalForm");
    
    if (event.targe == modal) {
        modal.style.display = "none";
    }
};

function redirectLogin() {
    location.replace("/login");
}

// funkce pro otevření modálního okna s mapou
function openMap() {
    var modal = document.getElementById("myModalMap");
    
    modal.style.display = "block";
}

function changeMapSrc(areaid) {
    var map = document.getElementById('taurus-czech-map');
    switch(areaid)
    {
        case 'plzensky':
            map.src = '../media/ns0eclvg/cs_plzensky.png';
            break;
        case 'jihocesky':
            map.src = '../media/2i3db4o4/cs_jihocesky.png';
            break;
        case 'jihomoravsky':
            map.src = '../media/mqpdovsr/cs_jihomoravsky.png';
            break;
        case 'zlinsky':
            map.src = '../media/1ixnzpg2/cs_zlinsky.png';
            break;
        case 'moravskoslezsky':
            map.src = '../media/ycjnfhqo/cs_morvaskoslezsky.png';
            break;
        case 'olomoucky':
            map.src = '../media/yenb1jxe/cs_olomoucky.png';
            break;
        case 'pardubicky':
            map.src = '../media/xecjizf5/cs_pardubicky.png';
            break;
        case 'kralovehradecky':
            map.src = '../media/tyvbfnv2/cs_kralovehradecky.png';
            break;
        case 'vysocina':
            map.src = '../media/egvhc2tx/cs_vysocina.png';
            break;
        case 'stredocesky':
            map.src = '../media/hpwfow3c/cs_stredocesky.png';
            break;
        case 'liberecky':
            map.src = '../media/h5zdq5lf/cs_liberecky.png';
            break;
        case 'ustecky':
            map.src = '../media/v14jki4n/cs_ustecky.png';
            break;
        case 'karlovarsky':
            map.src = '../media/moeb0ucg/cs_karlovarsky.png';
            break;
        case 'praha':
            map.src = '../media/baiphua3/cs_praha.png';
            break;
    }
}

function setMapSrc(e, areaid, b)
{
    e.preventDefault();
    
    var textInput = document.getElementById('taurus-czech-map-selected');
    textInput.value = areaid;
    
    if (b === true) {
        var additionaltext = document.getElementById('taurus-map-additionalplace-container');
        additionaltext.style.display = 'block';  
    }
    else {
        var form = document.getElementById('Taurus-StudentForm');
        form.style.display = 'block';
        
        form.scrollIntoView({behavior: "smooth", block: "end" });
    }
}

function leaveMapSrc() {
    var textInput = document.getElementById('taurus-czech-map-selected')
    var map = document.getElementById('taurus-czech-map');	         
     
    switch(textInput.value)
    {
        case 'plzensky':
            map.src = '../media/ns0eclvg/cs_plzensky.png';
            break;
        case 'jihocesky':
            map.src = '../media/2i3db4o4/cs_jihocesky.png';
            break;
        case 'jihomoravsky':
            map.src = '../media/mqpdovsr/cs_jihomoravsky.png';
            break;
        case 'zlinsky':
            map.src = '../media/1ixnzpg2/cs_zlinsky.png';
            break;
        case 'moravskoslezsky':
            map.src = '../media/ycjnfhqo/cs_morvaskoslezsky.png';
            break;
        case 'olomoucky':
            map.src = '../media/yenb1jxe/cs_olomoucky.png';
            break;
        case 'pardubicky':
            map.src = '../media/xecjizf5/cs_pardubicky.png';
            break;
        case 'kralovehradecky':
            map.src = '../media/tyvbfnv2/cs_kralovehradecky.png';
            break;
        case 'vysocina':
            map.src = '../media/egvhc2tx/cs_vysocina.png';
            break;
        case 'stredocesky':
            map.src = '../media/hpwfow3c/cs_stredocesky.png';
            break;
        case 'liberecky':
            map.src = '../media/h5zdq5lf/cs_liberecky.png';
            break;
        case 'ustecky':
            map.src = '../media/v14jki4n/cs_ustecky.png';
            break;
        case 'karlovarsky':
            map.src = '../media/moeb0ucg/cs_karlovarsky.png';
            break;
        case 'praha':
            map.src = '../media/baiphua3/cs_praha.png';
            break;
        default:
            map.src = '../media/s4emqbk1/cs_map.png';
    }
}