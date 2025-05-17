var errMsg = {
    require_err: 'Bạn chưa nhập thông tin này.',
    Fullname_empty: 'Bạn cần nhập Họ tên ( từ 2 đến 40 kí tự)',
    Account_empty: 'Bạn chưa nhập Tài khoản',
    Account_invalid: 'Vui lòng nhập Tài khoản từ 6-32 ký tự.',
    Gender_invalid: 'Vui lòng Chọn Giới tính',
    Date_invalid: 'Ngày tháng không hợp lệ.',
    Add_invalid: "Vui lòng nhập Địa chỉ từ 2-100 ký tự.",
    City_invalid: "Bạn chưa chọn Tỉnh Thành.",
    Occupation_invalid: "Bạn chưa chọn Nghề nghiệp.",
    marital_invalid: "Vui lòng Chọn Tình trạng hôn nhân.",
    Cmnd_Error: 'Vui lòng nhập số CMND (từ 8-15 ký tự).',
    Cmnd_Invalid: 'Số CMND không đúng định dạng.',
    Email_empty: 'Vui lòng nhập Email',
    ReEmail_empty: 'Vui lòng nhập lại Email',
    ReEmail_notmatch: 'Email xác nhận không trùng khớp.',
    Email_invalid: 'Định dạng email không hợp lệ.',
    Qna_err: 'Câu hỏi hoặc câu trả lời không đúng.',
    Qna_Empty_a: 'Vui lòng nhập câu trả lời ( < 100 ký tự ).',
    Qna_Empty_q: 'Bạn chưa chọn câu hỏi.',
    Qna_Error_confirm: 'Xác nhận câu trả lời không chính xác.',
    Pwd_empty: 'Vui lòng nhập mật khẩu',
    Pwd_invalid: 'Vui lòng nhập mật khẩu dài 6-32 ký tự, có ký tự chữ số, chữ hoa và chữ thường',
    RePwd_notmatch: 'Mật khẩu nhập lại không trùng khớp',
    Phone_invalid: 'Số điện thoại không đúng định dạng',
    Phone_notmatch: 'Chỉ hỗ trợ các đầu số Viettel, Mobifone, Vinaphone',
    Phone_empty: 'Vui lòng nhập số điện thoại'
}

/**
 * When focus on email, disapear error message.
 */
zm("#emailpro_input").focus(function() {
    if (this.value == "") {
        hideError("emailpro");
        zm('#emailpro_note').show();
    }
});

/**
 * When blur email field, hide note.
 */
zm("#emailpro_input").blur(function() {
    zm('#emailpro_note').hide();
});

/**
 * Show or hide answer: password or text format.
 * @returns {void}
 */
function showHideAnswer() {
    var sAnswer_old = zm('#sAnswer').val();
    var sAnswer_text = '<input type="text" onblur="onBlurAnswer();" onfocus="onFocusAnswer();" id="sAnswer" name="answer" maxlength="100" tabindex="19" class="input_login" value="' + sAnswer_old + '" />';
    var sAnswer_pass = '<input type="password" onblur="onBlurAnswer();" onfocus="onFocusAnswer();" id="sAnswer" name="answer" maxlength="100" tabindex="19" class="input_login" onchange="onChangeAnswer();" value="' + sAnswer_old + '" />';

    var flag = zm('#sAnswer_opt').html();
    if (flag == 'Ẩn chữ') {         // Display in format ****
        zm("#sAnswer_update").html(sAnswer_pass);
        zm('#sAnswer_opt').html('Hiện chữ');
        zm('#rowConfirmAnswer').show();
        zm('#sConfirmAnswer').val('');
    } else {                        // Display in alphabet characters
        zm("#sAnswer_update").html(sAnswer_text);
        zm('#sAnswer_opt').html('Ẩn chữ');
        zm('#rowConfirmAnswer').hide();
        zm('#sConfirmAnswer').val('');
    }
    zm("#sAnswer").focus();
}

/**
 * Reset sConfirmAnswer if new answer change.
 */
function onChangeAnswer() {
    zm("#sConfirmAnswer").val('');
    zm('#sAnswer_note').hide();
    hideError("sConfirmAnswer");
}

/**
 * When focus on answer again, disapear error message.
 */
function onFocusAnswer() {
    var answer = zm('#sAnswer').val();
    if (answer == '') {
        hideError("sAnswer");
        zm('#sAnswer_note').show();
    }
}
/**
 * Show valid icon if user input answer.
 */
function onBlurAnswer() {
    var answer = zm('#sAnswer').val();
    zm('#sAnswer_note').hide();
    if (answer != '') {
        hideError("sAnswer");
        showValidIcon("sAnswer");
    }
}

/**
 * Hide error message that is corresponding with an element. The element has to have an element to show error message.
 * @param {String} id - id of element
 * @returns {void}
 */
function hideError(id) {
    var errId = "#" + id + "_error";
    zm(errId).html("&nbsp;");
}
/**
 * Show element and fade out it in 2 seconds.
 * @param {String} id - id of element
 * @returns {Boolean} true
 */

function showValidIcon(id) {
    zm("#" + id + "_icon").show();
    zm("#" + id + "_icon").fadeOut(2000);
    return true;
}

//CONTENT
var zmIdInfo = {
    showError: function(id, content) {
        zm("#" + id).html(content);
        zm("#" + id).show();
        return false;
    },
    validEmail: function(email) {
        var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        return (filter.test(email));
    },
    validPhone : function(phone){
        var filter = /(0|84)+(90|91|93|94|96|97|98|120|121|122|123|124|125|126|127|128|129|162|163|164|165|166|167|168|169|86|88|89|32|33|34|35|36|37|38|39|70|76|77|78|79|81|82|83|84|85)\d{7}$/;
        return (filter.test(phone));
    },
    validAllPhone : function(phone){
        var filter = /^[0-9]{5,20}$/;
        return (filter.test(phone));
    },
    checkPersonalId: function(personalId) {
        if (personalId.length < 8 || personalId.length > 15) {
            return false;
        }
        if (!zmIdInfo.checkInputCharsCMND(personalId)) {
            return false;
        }
        return true;
    },
    checkInputCharsCMND: function(cmnd) {
        var pattern = /^[a-zA-Z0-9]*$/;
        if (cmnd.match(pattern) != null)
            return true;
        return false;
    },
    validRequireInfo: function() {
        zm(".Notice").html("&nbsp;");
        var isValid = true;

        //FullName
        var fullName = zm("#fullname_input").val();
        if (fullName == "" || fullName == " " ||fullName.length > 40 || fullName.length < 2) {
            zmIdContent.showError("fullname_error", errMsg.Fullname_empty);
            isValid = false;
        }
        // Gender
        var gender = zm('[name=gender]:checked').val();
        if (gender != 1 && gender != 0) {
            zmIdContent.showError("gender_error", errMsg.Gender_invalid);
            isValid = false;
        }

        // Dob
        var dob = zm('[name=dob]').val();
        var mob = zm('[name=mob]').val();
        var yob = zm('[name=yob]').val();
        var currentTime = new Date();
        var currentYear = currentTime.getFullYear();
        if (dob < 1 || dob > 31 || mob < 1 || mob > 12 || yob < 1900 || yob>currentYear) {
            zmIdContent.showError("dob_error", errMsg.Date_invalid);
            isValid = false;
        }

        // Add
        var add = zm('[name=add]').val();
        if (add == "" ||add.length > 100  || add.length < 2) {
            zmIdContent.showError("add_error", errMsg.Add_invalid);
            isValid = false;
        }

        // City
        var city = zm('[name=city]').val();
        if (city < 0) {
            zmIdContent.showError("city_error", errMsg.City_invalid);
            isValid = false;
        }

        // Occupation
        var occupation = zm('[name=occupation]').val();
        if (occupation < 0) {
            zmIdContent.showError("occupation_error", errMsg.Occupation_invalid);
            isValid = false;
        }

        // marital status
        var marital_status = zm('[name=marital_status]:checked').val();
        if (marital_status != 1 && marital_status != 0) {
            zmIdContent.showError("marital_status_error", errMsg.marital_invalid);
            isValid = false;
        }

        // CMND
        var cmnd_number = zm('[name=cmnd_number]').val();
        var docmnd = zm('[name=docmnd]').val();
        var mocmnd = zm('[name=mocmnd]').val();
        var yocmnd = zm('[name=yocmnd]').val();
        var citycmnd = zm('[name=citycmnd]').val();

        if (cmnd_number != undefined) { // User duoc yeu cau nhap CMND        
            if (cmnd_number == "" || cmnd_number == undefined) {
                zmIdContent.showError("cmnd_number_error", errMsg.Fullname_empty);
                isValid = false;
            }
            if (!zmIdInfo.checkPersonalId(cmnd_number)) {
                zmIdContent.showError("cmnd_number_error", errMsg.Cmnd_Invalid);
                isValid = false;
            }
            if (docmnd < 1 || docmnd > 31 || mocmnd < 1 || mocmnd > 12 || yocmnd < 1900 || yocmnd > currentYear) {
                zmIdContent.showError("cmnd_date_error", errMsg.Date_invalid);
                isValid = false;
            }
            if (citycmnd < 0) {
                zmIdContent.showError("cmnd_city_error", errMsg.City_invalid);
                isValid = false;
            }
        }

        // Email protect
        var emailpro = zm('[name=emailpro]').val();
        if (emailpro != undefined) { // User duoc yeu cau nhap email        
            if (emailpro == "") {
                zmIdContent.showError("emailpro_error", errMsg.Email_empty);
                isValid = false;
            }
            if (emailpro != "" && emailpro != undefined && !zmIdInfo.validEmail(emailpro)) {
                zmIdContent.showError("emailpro_error", errMsg.Email_invalid);
                isValid = false;
            }
        }
        
        // Phone protect
        var phonepro = zm('[name=phonepro]').val();
        if (phonepro != undefined) { // User duoc yeu cau nhap phone        
            if (phonepro == "") {
                zmIdContent.showError("phonepro_error", errMsg.Phone_empty);
                isValid = false;
            }
            if (phonepro != "" && phonepro != undefined && !zmIdInfo.validAllPhone(phonepro)) {
                zmIdContent.showError("phonepro_error", errMsg.Phone_invalid);
                isValid = false;
            }
            if (phonepro != "" && phonepro != undefined && zmIdInfo.validAllPhone(phonepro)&& !zmIdInfo.validPhone(phonepro)) {
                zmIdContent.showError("phonepro_error", errMsg.Phone_notmatch);
                isValid = false;
            }
        }


        // Qna
        var sAnswer = zm("#sAnswer").val();
        if (sAnswer != undefined) { // User duoc yeu cau nhap QnA  
            var selectQuestionId = zm("#questionId").val();
            if (selectQuestionId == -1) {
                zmIdContent.showError("questionId_error", errMsg.Qna_Empty_q);
                isValid = false;
            }

            if (sAnswer == '') {
                zmIdContent.showError("sAnswer_error", errMsg.Qna_Empty_a);
                isValid = false;
            }
            var sConfirmAnswer = zm("#sConfirmAnswer").val();
            if (sConfirmAnswer == '' && zm("#rowConfirmAnswer").css("display") != 'none') {
                zmIdContent.showError("sConfirmAnswer_error", errMsg.require_err);
                isValid = false;
            }

            if (sConfirmAnswer != '' && zm("#rowConfirmAnswer").css("display") != 'none' && sAnswer != sConfirmAnswer) {
                zmIdContent.showError('sConfirmAnswer_error', errMsg.Qna_Error_confirm);
                isValid = false;
            }
        }

        return isValid;
    }
}

function appendInfoRequireToken() {
    zm.getJSON("/ajax/gentoken", function(data) {
        if (data.code == "0") {
            //append token here
            zm("#inforequire_tokenExpire").val(data.msg);
        }
        else {
            location.reload();
        }
    });
}

function updateRequireInfo() {
    if (zmIdInfo.validRequireInfo())
    {
        zm("#info_loading").show();
        zmXCall.register('updateInfoCallback', function(data) {
            if (data.error == "0") {
                var content = '<div class="updatemsg"><span class="checkdoneicn"></span>Cập nhật thông tin thành công!</div>';
                zm.Boxy.alert(content, "", 1500, {
                    footer: false,
                    contentClass: 'title-alert'
                });
                setTimeout(function() {
                   location.reload();
                }, 1000);
            }
            else if (data.error == "-1") {
                zm.Boxy.alert(data.msg, "Thông báo", false, {
                    okButton: "Đóng",
                    onOk: reLoad
                });
                setTimeout(function() {
                    location.reload();
                }, 5000);
            }
            else {
                //show error
                var jsonStr = data.msg;
                var jsonArray = JSON.parse(jsonStr);
                appendInfoRequireToken();
                for (var i in jsonArray) {
                    var counter = jsonArray[i];
                    zm('[rel="' + counter.error + '"]').html(counter.msg);
                    zm("[rel=" + counter.error + "]").show();
                }
            }
        });
        zm("#frm_info").submit();
    }
    else {
        appendInfoRequireToken();
    }
    return false;
}
setTimeout(function() {
    appendInfoRequireToken();
    zm("#quesmark_qna").hover(function() {
        zm("#qna_tooltip").show();
    });
    zm("#quesmark_qna").mouseleave(function() {
        zm("#qna_tooltip").hide();
    });
    zm("#quesmark_gc").hover(function() {
        zm("#gc_tooltip").show();
    });
    zm("#quesmark_gc").mouseleave(function() {
        zm("#gc_tooltip").hide();
    });
    zm("#quesmark_cmnd").hover(function() {
        zm("#cmnd_tooltip").show();
    });
    zm("#quesmark_cmnd").mouseleave(function() {
        zm("#cmnd_tooltip").hide();
    });
    zm("#fullname_input").focus();


    //Personal
    zm("#fullname_input").focus(function() {
        zm('#fullname_error').html("&nbsp;");
    });
    zm('[name="gender"').click(function() {
        zm('#gender_error').html("&nbsp;");
    });
    zm('[name="dob"').focus(function() {
        zm('#dob_error').html("&nbsp;");
    });
    zm('[name="add"').focus(function() {
        zm('#add_error').html("&nbsp;");
    });
    zm('[name="city"').focus(function() {
        zm('#city_error').html("&nbsp;");
    });
    zm('[name="occupation"').focus(function() {
        zm('#occupation_error').html("&nbsp;");
    });
    zm('[name="marital_status"').click(function() {
        zm('#marital_status_error').html("&nbsp;");
    });

    //CMND
    zm('[name="cmnd_number"').focus(function() {
        zm('#cmnd_number_error').html("&nbsp;");
    });
    zm('[name="docmnd"').focus(function() {
        zm('#cmnd_date_error').html("&nbsp;");
    });
    zm('[name="citycmnd"').focus(function() {
        zm('#cmnd_city_error').html("&nbsp;");
    });

    // Sercurity Info
    zm('[name="phonepro"').focus(function() {
        zm('#phonepro_error').html("&nbsp;");
    });
    zm('[name="emailpro"').focus(function() {
        zm('#emailpro_error').html("&nbsp;");
    });
    zm('[name="questionId"').focus(function() {
        zm('#questionId_error').html("&nbsp;");
    });
    zm('[name="answer"').focus(function() {
        zm('#sAnswer_error').html("&nbsp;");
    });


    zm("#sAnswer").keypress(function(event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            updateRequireInfo();
        }
    });
}, 500)
