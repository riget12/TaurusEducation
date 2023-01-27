$(document).ready(function() {
    
    // odeslání na server dotaz, pro zobrazení správných krajů
    $.ajax({
        url: '/Umbraco/api/map/GetKrajeForMap',
        type: 'GET',
        cache: false,
        success: function(data) {
            var mapa = $('#image-map');
            var mapaAlternative = $('#mapClear-alternative-select');
            var kraje = JSON.parse(data.Data);
            
            mapa.empty();
            mapaAlternative.empty();
            
            $.each(kraje, function(i, val) {
               $('<area>')
                    .attr('onmouseover', 'changeMapSrc("' + val.Code + '")')
                    .attr('onclick', 'setMapSrc(event, "' + val.Code + '")')
                    .attr('onmouseout', 'leaveMapSrc()')
                    .attr('id', val.Code)
                    .attr('title', val.Name)
                    .attr('href', '')
                    .attr('coords', val.Coords)
                    .attr('shape', 'poly')
                    .appendTo(mapa);
                
                if (mapaAlternative.length !== 0) {
                     $('<option>')
                        .attr('value', val.Code)
                        .text(val.Name)
                        .appendTo(mapaAlternative);   
                }
            });
        }
    });

    var selectedKraj = $('#Kraj');
    if (selectedKraj.length === 0) {
        return;
    }

    var selValue = selectedKraj.val();
    if (selValue === '') {
        return;
    }

    setMap(selValue);
    changeMapSrc(selValue);
});

function changeMapSrc(areaid) {
    var map = document.getElementById('taurus-czech-map');
    switch(areaid.toLowerCase())
    {
        case 'plk':
            map.src = '../media/ns0eclvg/cs_plzensky.png';
            break;
        case 'jhc':
            map.src = '../media/2i3db4o4/cs_jihocesky.png';
            break;
        case 'jhm':
            map.src = '../media/mqpdovsr/cs_jihomoravsky.png';
            break;
        case 'zlk':
            map.src = '../media/1ixnzpg2/cs_zlinsky.png';
            break;
        case 'msk':
            map.src = '../media/ycjnfhqo/cs_morvaskoslezsky.png';
            break;
        case 'olk':
            map.src = '../media/yenb1jxe/cs_olomoucky.png';
            break;
        case 'pak':
            map.src = '../media/xecjizf5/cs_pardubicky.png';
            break;
        case 'hkk':
            map.src = '../media/tyvbfnv2/cs_kralovehradecky.png';
            break;
        case 'vys':
            map.src = '../media/egvhc2tx/cs_vysocina.png';
            break;
        case 'stc':
            map.src = '../media/hpwfow3c/cs_stredocesky.png';
            break;
        case 'lbk':
            map.src = '../media/h5zdq5lf/cs_liberecky.png';
            break;
        case 'ulk':
            map.src = '../media/v14jki4n/cs_ustecky.png';
            break;
        case 'kvk':
            map.src = '../media/moeb0ucg/cs_karlovarsky.png';
            break;
        case 'pha':
            map.src = '../media/baiphua3/cs_praha.png';
            break;
    }
}

function leaveMapSrc() {
    var textInput = document.getElementById('taurus-czech-map-selected');
    var map = document.getElementById('taurus-czech-map');	         
     
    switch(textInput.value.toLowerCase())
    {
        case 'plk':
            map.src = '../media/ns0eclvg/cs_plzensky.png';
            break;
        case 'jhc':
            map.src = '../media/2i3db4o4/cs_jihocesky.png';
            break;
        case 'jhm':
            map.src = '../media/mqpdovsr/cs_jihomoravsky.png';
            break;
        case 'zlk':
            map.src = '../media/1ixnzpg2/cs_zlinsky.png';
            break;
        case 'msk':
            map.src = '../media/ycjnfhqo/cs_morvaskoslezsky.png';
            break;
        case 'olk':
            map.src = '../media/yenb1jxe/cs_olomoucky.png';
            break;
        case 'pak':
            map.src = '../media/xecjizf5/cs_pardubicky.png';
            break;
        case 'hkk':
            map.src = '../media/tyvbfnv2/cs_kralovehradecky.png';
            break;
        case 'vys':
            map.src = '../media/egvhc2tx/cs_vysocina.png';
            break;
        case 'stc':
            map.src = '../media/hpwfow3c/cs_stredocesky.png';
            break;
        case 'lbk':
            map.src = '../media/h5zdq5lf/cs_liberecky.png';
            break;
        case 'ulk':
            map.src = '../media/v14jki4n/cs_ustecky.png';
            break;
        case 'kvk':
            map.src = '../media/moeb0ucg/cs_karlovarsky.png';
            break;
        case 'pha':
            map.src = '../media/baiphua3/cs_praha.png';
            break;
        default:
            map.src = '../media/s4emqbk1/cs_map.png';
    }
}

// nastaví oblast mapy po kliknutí na ni
function setMapSrc(e, areaid)
{
    e.preventDefault();
    
    setMap(areaid);
}

function setMap(areaid) {
    var textInput = document.getElementById('taurus-czech-map-selected');
    textInput.value = areaid;
    
    var map = $('#image-map');
    if (map.lentgh === 0) {
        return;
    }
    
    var student = map.attr('data-student');
    var form;
    if (student === 'True') {
        form = document.getElementById('Taurus-StudentForm');
    } else {
        form = document.getElementById('Taurus-NewLectorForm');
    }

    var krajInput = $('#Kraj');
    if (krajInput.length !== 0) {
        krajInput.val(areaid);
    }

    form.style.display = 'block';
    form.scrollIntoView({behavior: "smooth", block: "end" });
    
    // odeslání na server dotaz, pro zobrazení správných okresů
    $.ajax({
        url: '/Umbraco/api/map/GetOkresy',
        type: 'GET',
        cache: false,
        data: { 
            kod: areaid },
        success: function(data) {
            var okresywrapper = $('#okresy-content');
            var okresy = JSON.parse(data.Data);
            var list = $('<ul>').addClass('okresy-list');
            
            okresywrapper.empty();

            var selectedOkresyInput = $('#Okres');
            var selectedOkresy;
            if (selectedOkresyInput !== 0) {
                var input = selectedOkresyInput.val();
                selectedOkresy = input.split(';');
            }

            Object.keys(okresy)
                .forEach(function eachKey(key) {
                    var selected = selectedOkresy.includes(key);

                   $('<li>').attr({
                       'data-key': key,
                       'data-value': okresy[key],
                       'id': key,
                       'data-selected': selected
                   }).text(okresy[key]).addClass('okres').addClass((selected === true ? 'okres-active' : '')).on('click', function () {
                       var attr = $(this).attr('data-selected');
                       var select = false;
                       if (attr === undefined || attr === 'false') {
                           $(this).attr('data-selected', true).addClass('okres-active');
                           select = true;
                       } else {
                           $(this).attr('data-selected', false).removeClass('okres-active');
                           select = false;
                       }
                       
                       var okresInput = $('#Okres');
                       if (okresInput.length !== 0) {
                           var seznamOkresu = okresInput.val();
                           if (select === true) {
                               seznamOkresu = seznamOkresu + ';' + $(this).attr('data-key');
                           }
                           else {
                               seznamOkresu = seznamOkresu.replace($(this).attr('data-key'), '');
                           }
                           
                           okresInput.val(seznamOkresu);
                       }
                   }).appendTo(list);
                });
            list.appendTo(okresywrapper);
        }
   });
}