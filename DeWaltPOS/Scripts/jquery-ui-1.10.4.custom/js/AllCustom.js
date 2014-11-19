$(document).ready(function () {
    //$('#Addrel').hide();



    //For Create and Edit screens for products, if Product type is Display Fixture (Type-2 then show panel choice dropdown, for other kinds of products show related panels
    if ($('#prodtype').val() == 2) {
        $('#form-group-product-panel-type').show();
        $('#edit-related-panels').hide();
    }
    else
    {
        $('#form-group-product-panel-type').hide();
        $('#edit-related-panels').show();
    }

    //not used
    $('#DCreate').on('click', function(e)
    {
        var dval = $('#dsku').val();
        var nid = "";

        $.ajax({
            url: "/SimpleICMS/GetDeWaltProd?sku=" + dval,
            type: "POST",
            dataType: 'json',
            success: function (response) {
                nid = response.id;
                var nsku = response.sku;
                var redirectpth = '/DeWaltProduct/';

                var rwhtml = '<tr><td>';
                rwhtml += nsku + '</td><td>';
                rwhtml += '<a href="' + redirectpth + 'Edit/' + nid + '">Edit</a> | ';
                rwhtml += '<a href="' + redirectpth + 'Delete/' + nid + '">Delete</a>';
                rwhtml+= '</td></tr>'
                $('tbody').append(rwhtml);
                $('#dsku').val("");
            },
            error: function (er) {

                alert(er);
            }
        });


        $.ajax({
            type: 'POST',
            url: '/DewaltProduct/NonRelatedProducts?id=' +  nid,
            dataType: 'html',
            success: function (data) {
                $('#partialvw').innerHTML = data;
            }
        });

    });


    //not used
    $('#DAdd').on('click', function (e) {
        var nid = "";

        $.ajax({
            type: 'POST',
            url: '/DewaltProduct/NonRelatedProducts?id=50',
            success: function (dta) {
                $('#partialvw').html(dta);
            }
        });

    });


    //not used
    $('#btnaddrel').on('click', function (e) {
        $('#Addrel').show();
    });

    //not used
    $('.tabs .tab-links a').on('click', function (e) {
        var currentAttrValue = $(this).attr('href');

        // Show/Hide Tabs
        $('.tabs ' + currentAttrValue).show().siblings().hide();

        // Change/remove current tab to active
        $(this).parent('li').addClass('active').siblings().removeClass('active');

        e.preventDefault();
    });

    //not used
    $('#prodtype').change(function () {
        if ($(this).val() == 1) {
            $('.superdescription').show();
            $('.normalproduct').hide();
            $('#length').val() = 0;
        }
        else {
            $('.superdescription').hide();
            $('.normalproduct').show();
        }
    });

    //Allow only numbers and 1 .(for decimal) to be entered in these class of fields
    $(".num").keydown(function (e) {
        var txt = $(this).val();
        // Allow: backspace, delete, tab, escape, enter and .
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110]) !== -1 ||
            // Allow: Ctrl+A, Ctr+C, Ctrl+v
            (e.keyCode == 67 && e.ctrlKey === true) ||
            (e.keyCode == 86 && e.ctrlKey === true) ||
            (e.keyCode == 65 && e.ctrlKey === true) ||
            // Allow: home, end, left, right
            (e.keyCode >= 35 && e.keyCode <= 39)) {
            // let it happen, don't do anything
            return;
        }
        if ((txt.indexOf(".") < 0) && ( e.keyCode == 190))
        {
            return;
        }

        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });

    //Allow int only and no decimal to be entered in these class of fields
    $(".int-only").keydown(function (e) {
        var txt = $(this).val();
        // Allow: backspace, delete, tab, escape, enter and .
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110]) !== -1 ||
            // Allow: Ctrl+A, Ctr+C, Ctrl+v
            (e.keyCode == 67 && e.ctrlKey === true) ||
            (e.keyCode == 86 && e.ctrlKey === true) ||
            (e.keyCode == 65 && e.ctrlKey === true) ||
            // Allow: home, end, left, right
            (e.keyCode >= 35 && e.keyCode <= 39)) {
            // let it happen, don't do anything
            return;
        }

        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });

    var unsavedflag = 0;
    //Check flag to see if anything has changed in Edit or Create field
    $('input').keyup(function () {
        unsavedflag = 1;
    });

    //Check flag to see if anything has changed in Edit or Create field
    $('#btnaddimg').click(function()
    {
        unsavedflag = 1;
    });

    //Check flag to see if anything has changed in Edit or Create field
    $('input').change(function () {
        unsavedflag = 1;
    });

    //Check flag to see if anything has changed in Edit or Create field
    $('.btn-danger').click(function () {
        unsavedflag = 1;
    });

    //Check flag to see if anything has changed in Edit or Create field
    $('select').on("change", function () {
        unsavedflag = 1;
    });

    //Check flag to see if anything has changed in Edit or Create field
    $("button[type='submit']").click(function (e) {
        var skuval = $("input[name='sku']").val();
        if (skuval == "")
        {
            alert("Please Enter a SKU value");
            e.preventDefault();
        }
        unsavedflag = 0;
    });

    // Alert with confirm button if navigating away from a page which is being edited or added
    $(window).bind('beforeunload', function () {
        var isedit = $('editpage').html()

        $.get("/SimpleICMS/DeleteTempImages", function () { });

        if ((unsavedflag != 0) && (isedit == 1)) {
            return "There are unsaved data on this page. Are you sure you want to leave without saving";

        }
    });


});

//Add Bootstrap class 'modal-open' when bootstrap modal window for related panel or product selection is opened
$('#btn-add-product').on('click', function () {
    $('body').addClass('modal-open');
});

//Remove Bootstrap class 'modal-open' when bootstrap modal window for related panel or product selection is closed
$('#add-product-modal').on('hidden.bs.modal', function () {
    $('body').removeClass('modal-open');
})

//On the modal windows for choosing related products, show or hide the skus for the list that matches the sku search text in the search textbox on top
$('#product-add-search').on('keyup', function () {
    var srchtext = $(this).val();

    var sku_names = $('.row .sku-name');
    if (srchtext == "")
    {
        sku_names.each(function () {
            $(this).parent().parent().css({ 'display': 'normal' });
        });
    }
    else
    {
        sku_names.each(function () {
            var i_skuname = $(this).html();
            if(i_skuname.toUpperCase().indexOf(srchtext.toUpperCase()) >= 0)
            {
                $(this).parent().parent().css({ 'display': 'normal' });
                //alert(i_skuname);
            }
            else {
                $(this).parent().parent().css({ 'display': 'none' });
            }
        });
    }


});

//In the Edit and Create pages show and hide certain input fields according to whether it is a display fixture or other type
//For display fixture show the panel type choice for others show the related products choice
$('#prodtype').on('change', function () {
    if ($('#prodtype').val() == 2) {
        $('#form-group-product-panel-type').show();
        $('#edit-related-panels').hide();
    }
    else {
        $('#form-group-product-panel-type').hide();
        $('#edit-related-panels').show();
    }

});

$('.dsku').on('mouseover', function (e) {
    var nid = $(this).attr('tag');

    $.ajax({
        type: 'POST',
        url: '/DewaltProduct/NonRelatedProducts?id=' + nid,
        success: function (dta) {
            $('#partialvw').html(dta);

            $("input[type='checkbox']").click(function () {
                var status = $(this).prop('checked');
                var aid = $(this).attr('id');
                aid = "." + aid + "Chld " + "input";
                $(aid).each(function () {
                    // Set the checked status of each to match the 
                    // checked status of the check all checkbox:
                    $(this).prop("checked", status);
                });
            });

        }
    });




});

//On the Index pages for all the different products search by sku name. The 'typ' field gets the type of product it is and then renders the partial view for the type of product accrodingly
// The value in 'typ' is fetched for the attribute 'tag' in the search textbox field
$('#srch').on('keyup', function (e) {
    var srch = $(this).val();
    var typ = $(this).attr('tag');
    //var cattype = ('#prodtypesrch').val();
    $.ajax({
        type: 'POST',
        url: '/DewaltProduct/ProductDisplay?sku=' + srch + "&type=" + typ,
        success: function (dta) {
            $('#DispProd').html(dta);
        }
    });
});

//On the index page for display products search by Product Type in Dropdown
$('#prodtypesrch').on('change', function (e) {
    var typ = $('#srch').attr('tag');
    var cattype = $(this).val();
    $.ajax({
        type: 'POST',
        url: '/DewaltProduct/ProductDisplay?cat=' + cattype + "&type=" + typ,
        success: function (dta) {
            $('#DispProd').html(dta);
        }
    });
});

//On the index page for display fixtures search by Panel Type in Dropdown
$('#paneltypesrch').on('change', function (e) {
    var srch = $(this).val();
    $.ajax({
        type: 'POST',
        url: '/DewaltProduct/ProductDisplay?paneltype=' + srch + "&type=2",
        success: function (dta) {
            $('#DispProd').html(dta);
        }
    });
});

//On the index page for recommended products search by Best Practice type in Dropdown
$('#besttypesrch').on('change', function (e) {
    var srch = $(this).val();
    $.ajax({
        type: 'POST',
        url: '/DewaltProduct/ProductDisplay?besttype=' + srch + "&type=1",
        success: function (dta) {
            $('#DispProd').html(dta);
        }
    });
});

//This code if copied can be reused for image upload control
//This enables the image to be shown on the div whenever the fileupload control is used for uploading images 
$('.editpic').change(function () {

    var data = new FormData();
    var elem = $(this);
    var files = $(this).get(0).files;
    if (files.length > 0) {
        data.append("addpic", files[0]);
    }
    $.ajax({
        url: "/SimpleICMS/AddImage",
        type: "POST",
        processData: false,
        contentType: false,
        data: data,
        success: function (response) {
            $('#picpath').html(response.pth);
            $('#thumbimg').attr('src', response.pth);
            var backurl = "url('" + response.pth + "')";
            elem.closest('div').css({ "background": backurl, 'background-size': 'contain', 'background-repeat': 'no-repeat', 'display': 'table-cell', 'background-position': '50% 50%' });

        },
        error: function (er) {
            $('#picpath').html("error");
            alert(er);
        }

    });
});

//The custom image upload control is generated on screen asynchronously whenever this button with id 'btnaddimg' is clicked
$('#btnaddimg').on('click', function () {
    var outerdiv = '<div style="float:left;border-width:3px;border-style:groove;border-color:darkgrey">';
    var caption = '<p>Click on the Image below to Add/Upload a picture</p>'
    var editpichtml = '<div class="addallsuperctlg" style="float:left;border-width:2px;border-color:black;border-style:groove"><div class="custpicupl" style="background-color:black"><input type="file" name="cfiles" id="cfile" value="" class="editpic" /></div></div><br/>';
    var delpichtml = '<div style="width:100%; text-align:center; cursor:pointer" href=" #" class="btn-danger btn-xs btn-block btn-temp-del"><div class="del-restore"><span class="glyphicon glyphicon-remove"></span><span> Delete image</span></div><input type="checkbox" name="delnewctlg" style="display:none"/></div>';
    var enddiv = '</div>';
    var newhtml = outerdiv + caption + editpichtml + delpichtml + enddiv;
    $("html, body").animate({ scrollTop: $(document).height() }, 1000);
    $('.addeditsuperctlg').prepend(newhtml);
    $('.editpic').change(function () {

        var data = new FormData();
        var elem = $(this);
        var files = $(this).get(0).files;
        if (files.length > 0) {
            data.append("addpic", files[0]);
        }
        $.ajax({
            url: "/SimpleICMS/AddImage",
            type: "POST",
            processData: false,
            contentType: false,
            data: data,
            success: function (response) {
                $('#picpath').html(response.pth);
                $('#thumbimg').attr('src', response.pth);
                var backurl = "url('" + response.pth + "')";
                elem.closest('div').css({ "background": backurl, 'background-size': 'contain', 'background-color':'white', 'background-repeat': 'no-repeat', 'display': 'table-cell', 'background-position': '50% 50%' });

                $('.btn-temp-del').on('click', function (e) {
                    e.preventDefault();
                    var delhtml = '<span class="glyphicon glyphicon-remove"></span><span> Delete image</span>';
                    var restorehtml = '<span> Restore image</span>'
                    var delchk = $(this).find('input[type="checkbox"]');
                    if (delchk.prop('checked') == true) {
                        delchk.prop('checked', false);
                        $(this).find('div').html(delhtml);
                        $(this).siblings('div').css({ 'opacity': '1' });
                        $(this).siblings('div').find('input[type="file"]').attr('name', 'cfiles');
                        $(this).siblings('div').find('input[type="file"]').prop('disabled', false);
                    }
                    else {
                        delchk.prop('checked', true);
                        $(this).find('div').html(restorehtml);
                        $(this).siblings('div').css({ 'opacity': '0.2' });
                        $(this).siblings('div').find('input[type="file"]').removeAttr('name');
                        $(this).siblings('div').find('input[type="file"]').prop('disabled', true);


                    }


                });


            },
            error: function (er) {
                $('#picpath').html("error");
                alert(er);
            }

        });
    });
});

//not used
$(".supeditpic").change(function ()
{
    var data = new FormData();
    var elem = $(this);
    var files = $(this).get(0).files;
    if (files.length > 0) {
        data.append("addpic", files[0]);
    }
    $.ajax({
        url: "/SimpleICMS/AddImage",
        type: "POST",
        processData: false,
        contentType: false,
        data: data,
        success: function (response) {
            $('#picpath').html(response.pth);
            $('#thumbimg').attr('src', response.pth);
            var backurl = "url('" + response.pth + "')";
            var newhtml = '<div style="float:left;border-width:3px;border-style:groove;border-color:darkgrey"><div class="custpicupl" style="background:' + backurl + '; background-size:100% 100%"></div><br /></div>';
            var divhtml = elem.closest('.addsuperctlg').html();
            var divscript = elem.closest('.addsuperctlg').getElementsByTagName("script");
            alert(divscript);
            $('.addeditsuperctlg').append(divhtml);
            elem.closest('div').css({ "background": backurl, 'background-size': '100% 100%' });

        },
        error: function (er) {
            $('#picpath').html("error");
            alert(er);
        }

    });
});

//not used
$('#ProdList').change(function () {
    var dta = $(this).val();

    $.ajax({
        url: "/SimpleICMS/GetProdData?pid=" +dta,
        type: "POST",
        dataType: 'json',
        success: function (response) {
            $('#picpath').html(response.thumbpth);
            $('#thumbimg').attr('src', response.thumbpth);
            var backturl = "url('" + response.thumbpth + "')";
            var backcurl = "url('" + response.catlgpath + "')";

            $('#tfile').closest('div').css({ "background": backturl, 'background-size': '100% 100%' });
            $('#cfile').closest('div').css({ "background": backcurl, 'background-size': '100% 100%' });


        },
        error: function (er) {
            $('#picpath').html("error");
            alert(er);
        }

    });
});

//not used
$('#ProductDetList').change(function () {
    var dta = $(this).val();

    $.ajax({
        url: "/SimpleICMS/GetProdData?pid=" + dta,
        type: "POST",
        dataType: 'json',
        success: function (response) {
            $('#picpath').html(response.thumbpth);
            $('#thumbimg').attr('src', response.thumbpth);
            var backturl = "url('" + response.thumbpth + "')";
            var backcurl = "url('" + response.catlgpath + "')";

            $('#tfile').closest('div').css({ "background": backturl, 'background-size': '100% 100%' });
            $('#cfile').closest('div').css({ "background": backcurl, 'background-size': '100% 100%' });


        },
        error: function (er) {
            $('#picpath').html("error");
            alert(er);
        }

    });
});

//If a Checkall checkbox is clicked then all the child checkbox have to be selected or deselected accordingly. The childen are identified by their classname which is id of parent checkbox + 'chld'
    $("input[type='checkbox']").click(function () {
        var status = $(this).prop('checked');
        var aid = $(this).attr('id');   
        aid = "." + aid + "Chld " + "input";
        $(aid).each(function () {
            // Set the checked status of each to match the 
            // checked status of the check all checkbox:
            $(this).prop("checked", status);
        });
    });

//Making the current selected menu item active and the others inactive. This helps in creating the menu highlight
    $('ul.list-group-item li a').on('click', function () {
        $(this).addClass('active');
        $(this).siblings().removeClass('active');
    });

//not used. Just to check if jquery is loaded on the page
    $('#testjq').on('click', function()
    {
        alert('JQUERY ALIVE')
    });

//When panels are selected or deleted then, they are shown as strikeout and greyed background and identified with a checkbox.
    $('.remove-panel').on('click', function () {

        var elem = $(this);
        var status = elem.parent().siblings('div').find("input[type='checkbox']").prop('checked');
        if (status == true)
        {
            elem.parent().siblings('div').css({ "text-decoration": 'none', 'background-color': 'transparent' });
            elem.parent().removeClass('col-xs-2');
            elem.parent().addClass('col-xs-1');
            elem.html('Delete');
            elem.parent().siblings('div').find("input[type='checkbox']").prop('checked', false);
            elem.parent().siblings('div').find("input[type='text']").prop('disabled', false);
            elem.parent().siblings('div').find("input[type='text']").css({ "text-decoration": 'none', 'background-color': 'transparent' });
            elem.parent().parent().css({ 'background-color': 'transparent' });

        }
        else
        {
            elem.parent().siblings('div').css({ "text-decoration": 'line-through', 'background-color': 'lightgray' });
            elem.parent().removeClass('col-xs-1');
            elem.parent().addClass('col-xs-2');
            elem.html('Cancel Deletion');
            elem.parent().siblings('div').find("input[type='checkbox']").prop('checked', true);
            elem.parent().siblings('div').find("input[type='text']").prop('disabled', true);
            elem.parent().siblings('div').find("input[type='text']").css({ "text-decoration": 'line-through', 'background-color': 'lightgray' });
            elem.parent().parent().css({ 'background-color': 'lightgray' });
        }

    });

//When products are selected or deleted then, they are shown as strikeout and greyed background and identified with a checkbox.
    $('.remove-rel-prod').on('click', function () {

        var elem = $(this);
        var status = elem.parent().siblings('div').find(".nchk").prop('checked');
        if (status == true) {
            elem.parent().siblings('div').css({ "text-decoration": 'none', 'background-color': 'transparent' });
            elem.parent().removeClass('col-xs-2');
            elem.parent().addClass('col-xs-1');
            elem.html('Delete');
            elem.parent().siblings('div').find(".nchk").prop('checked', false);
            elem.parent().siblings('div').find("input[type='text']").prop('disabled', false);
            elem.parent().siblings('div').find("input[type='text']").css({ "text-decoration": 'none', 'background-color': 'transparent' });
            elem.parent().parent().css({ 'background-color': 'transparent' });

        }
        else {
            elem.parent().siblings('div').css({ "text-decoration": 'line-through', 'background-color': 'lightgray' });
            elem.parent().removeClass('col-xs-1');
            elem.parent().addClass('col-xs-2');
            elem.html('Cancel Deletion');
            elem.parent().siblings('div').find(".nchk").prop('checked', true);
            elem.parent().siblings('div').find("input[type='text']").prop('disabled', true);
            elem.parent().siblings('div').find("input[type='text']").css({ "text-decoration": 'line-through', 'background-color': 'lightgray' });
            elem.parent().parent().css({ 'background-color': 'lightgray' });
        }

    });

//When quantity is updated asynchronously for best practice - related products
    $('.upd-qty').on('keyup', function () {

        var elem = $(this);
        elem.parent().siblings('div').find(".uchk").prop('checked', true);

    });


//When related panels are added they are shown asynchronously through partial view Temprelated panels. These are updated in the database only when the save button is clicked
    $("#btn-add-panels").on('click', function () {
        var chks = $(".NRelPnlChkChld input[type='checkbox']");
        $('#rel-panels-temp').html("");
        chks.each(function () {
            if($(this).prop('checked') == true)
            {
                var vid = $(this).attr('value');
                $.ajax({
                    type: 'POST',
                    url: '/DewaltProduct/TempRelatedPanels?panelid=' + vid,
                    success: function (dta) {
                        $('#rel-panels-temp').append(dta);
                        $('.temp-remove-panel').on('click', function () {
                            var elem = $(this);
                            var status = elem.parent().siblings('div').find("input[type='checkbox']").prop('checked');
                            var thisid = elem.parent().siblings('div').find("input[type='checkbox']").attr('value');
                            if (status == true) {
                                elem.parent().siblings('div').css({ "text-decoration": 'none', 'background-color': 'transparent' });
                                elem.parent().removeClass('col-xs-2');
                                elem.parent().addClass('col-xs-1');
                                elem.html('Delete');
                                var chkchoice = $(".NRelPnlChkChld input[type='checkbox']");
                                chkchoice.each(function () {
                                    if($(this).attr('value') == thisid)
                                    {
                                        $(this).prop('checked', true);
                                    }
                                });


                            }
                            else {
                                elem.parent().siblings('div').css({ "text-decoration": 'line-through', 'background-color': 'lightgray' });
                                elem.parent().removeClass('col-xs-1');
                                elem.parent().addClass('col-xs-2');
                                elem.html('Cancel Deletion');
                                var chkchoice = $(".NRelPnlChkChld input[type='checkbox']");
                                chkchoice.each(function () {
                                    if ($(this).attr('value') == thisid) {
                                        $(this).prop('checked', false);
                                    }
                                });

                            }
                        });

                    }
                });
            }
        });

    });

    //For add related prduct modal window the when the add button of each individual product is added then the record gets highlighted 
    //and the quantity textbox becomes visible
    $('.add-prod-relation').on('click', function () {

        var elem = $(this);
        var status = elem.parent().siblings('div').find("input[type='checkbox']").prop('checked');
        if (status == true) {
            elem.parent().siblings('div').css({  'background-color': 'transparent' });
            elem.html('Add');
            elem.parent().siblings('div').find("input[type='checkbox']").prop('checked', false);
            elem.parent().siblings('div').find("input[type='text']").parent().css({ 'display': 'none' });
            elem.parent().parent().css({ 'background-color': 'transparent' });

        }
        else {
            //elem.parent().siblings('div').css({  'background-color': 'lightgreen' });
            elem.html('Remove');
            elem.parent().siblings('div').find("input[type='checkbox']").prop('checked', true);
            elem.parent().siblings('div').find("input[type='text']").parent().css({'display':'normal'});
            elem.parent().parent().css({ 'background-color': 'lemonchiffon' });
        }

    });

    //When related products are added they are shown asynchronously through partial view Temprelatedproducts. 
    //These can then be updated deleted or readded. These are updated in the database only when the save button is clicked
    $("#product-relation-add-button").on('click', function () {
        var type = $('#type').val();
        var chks = $(".NRelChkChld input[type='checkbox']");
        var rids = [];
        var qtys = [];
        chks.each(function () {
            if ($(this).prop('checked') == true) {
                var vid = $(this).attr('value');
                var relqty = $(this).parent().parent().siblings('div').find("#relqty").val();

                rids.push(vid);
                qtys.push(relqty);
            }
        });
        var list = '{rids : [' + rids + '], qtys : [' + qtys + '], type : "' + type + '"}'
        $.ajax({
            type: 'POST',
            data: list,
            traditional: true,
            contentType: "application/json; charset=utf-8",
            url: '/DewaltProduct/TempRelatedProducts/',
            success: function (dta) {
                $('#rel-prods-temp').html("");
                $('#rel-prods-temp').append(dta);


                $('.temp-remove-product').on('click', function () {
                    var elem = $(this);
                    var status = elem.parent().siblings('div').find("input[type='checkbox']").prop('checked');
                    var thisid = elem.parent().siblings('div').find("input[type='checkbox']").attr('value');
                    if (status == true) {
                        elem.parent().siblings('div').css({ "text-decoration": 'none' });
                        elem.parent().siblings('div').find("input[type='text']").prop('disabled', false);
                        elem.parent().siblings('div').find("input[type='text']").css({ 'background-color': 'transparent', "text-decoration": 'none' });
                        elem.parent().parent().css({ 'background-color': 'transparent' });
                        elem.parent().siblings('div').find("input[type='checkbox']").prop('checked', false);
                        elem.parent().removeClass('col-xs-2');
                        elem.parent().addClass('col-xs-1');
                        elem.html('Delete');
                        var chkchoice = $(".NRelChkChld input[type='checkbox']");
                        chkchoice.each(function () {
                            if ($(this).attr('value') == thisid) {
                                $(this).prop('checked', true);
                                var elemchk = $(this).parent();
                                elemchk.parent().siblings('div').find(".add-prod-relation").html('Remove');
                                elemchk.parent().siblings('div').find("input[type='checkbox']").prop('checked', true);
                                elemchk.parent().siblings('div').find("input[type='text']").parent().css({ 'display': 'normal' });
                                elemchk.parent().parent().css({ 'background-color': 'lemonchiffon' });

                            }
                        });
                    }
                    else {
                        elem.parent().parent().css({'background-color': 'lightgray' });
                        elem.parent().siblings('div').css({ "text-decoration": 'line-through' });
                        elem.parent().siblings('div').find("input[type='text']").prop('disabled', true);
                        elem.parent().siblings('div').find("input[type='text']").css({ 'background-color': 'lightgray', "text-decoration": 'line-through' });
                        elem.parent().siblings('div').find("input[type='checkbox']").prop('checked', true);
                        elem.parent().removeClass('col-xs-1');
                        elem.parent().addClass('col-xs-2');
                        elem.html('Cancel Deletion');
                        var chkchoice = $(".NRelChkChld input[type='checkbox']");
                        chkchoice.each(function () {
                            if ($(this).attr('value') == thisid) {
                                $(this).prop('checked', false);
                                var elemchk = $(this).parent();
                                elemchk.parent().siblings('div').find(".add-prod-relation").html('Add');
                                elemchk.parent().parent().css({ 'background-color': 'transparent' });
                                elemchk.prop('checked', false);
                                elemchk.parent().siblings('div').find("input[type='text']").parent().css({ 'display': 'none' });


                            }
                        });
                    }
                });

                $(".temp-quantity").on('keyup', function () {
                    var qtyval = $(this).val();
                    var this_id = $(this).attr('tag');
                    $(".clsrelqty").each(function (index, el) {
                        var tagval = $(this).attr('name');
                        if (tagval == this_id) {
                            $(this).val(qtyval);

                        }
                    });

                });

                $(".int-only").on('keydown', function (e) {
                    var txt = $(this).val();
                    // Allow: backspace, delete, tab, escape, enter and .
                    if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110]) !== -1 ||
                        // Allow: Ctrl+A, Ctr+C, Ctrl+v
                        (e.keyCode == 67 && e.ctrlKey === true) ||
                        (e.keyCode == 86 && e.ctrlKey === true) ||
                        (e.keyCode == 65 && e.ctrlKey === true) ||
                        // Allow: home, end, left, right
                        (e.keyCode >= 35 && e.keyCode <= 39)) {
                        // let it happen, don't do anything
                        return;
                    }

                    // Ensure that it is a number and stop the keypress
                    if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                        e.preventDefault();
                    }
                });

            }
        });


    });

//For the multiple catalogue images in Super sku product, when an image is deleted then it gets obscured and is tagged with a checkbox for backed processing
    $('.btn-ctlg-delete-image').on('click', function (e) {
        var delhtml = '<span class="glyphicon glyphicon-remove"></span><span> Delete image</span>';
        var restorehtml = '<span> Restore image</span>'
        e.preventDefault();
        var delchk = $(this).find('input[type="checkbox"]');
        if( delchk.prop('checked') == true)
        {
            delchk.prop('checked', false);
            $(this).find('div').html(delhtml);
            $(this).siblings('div').css({ 'opacity': '1' });
        }
        else
        {
            delchk.prop('checked', true);
            $(this).find('div').html(restorehtml);
            $(this).siblings('div').css({ 'opacity': '0.2' });
            $(this).siblings('div input').prop('disabled', true);

        }


    });



    