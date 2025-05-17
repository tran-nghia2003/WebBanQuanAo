var contact = {
    init: function () {
        contact.registerEvents();
    },
    registerEvents: function () {
        $('#btnSend').off('click').on('click', function () {
            var name = $('#txtName').val();
            var mobile = $('#txtMobile').val();
            var address = $('#txtAddress').val();
            var email = $('#txtEmail').val();
            var content = $('#txtContent').val();

            // Reset previous error messages
            $('.error-message').text('');

            // Check for empty fields
            if (!name || !mobile || !address || !email || !content) {
                // Display error messages
                if (!name) $('#errorFullname').text("Chưa nhập tên!");
                if (!mobile) $('#errorPhone').text("Chưa nhập số điện thoại!");
                if (!address) $('#errorAddress').text("Chưa nhập địa chỉ!");
                if (!email) $('#errorMail').text("Chưa nhập email!");
                if (!content) $('#errorMessage').text("Chưa nhập nội dung!");
                return; // Do not proceed with form submission
            }
            else
            {
                $.ajax({
                    url: '/Contact/Send',
                    type: 'POST',
                    dataType: 'json',
                    data: {
                        name: name,
                        mobile: mobile,
                        address: address,
                        email: email,
                        content: content
                    },
                    success: function (res) {
                        if (res.status == true) {
                            window.alert('Gửi thành công');
                            contact.resetForm();
                        }
                    }
                });
            }

            // If all fields are filled, proceed with AJAX request
            
        });
    },
    resetForm: function () {
        $('#txtName').val('');
        $('#txtMobile').val('');
        $('#txtAddress').val('');
        $('#txtEmail').val('');
        $('#txtContent').val('');
        // Reset error messages
        $('.error-message').text('');
    }
}
contact.init();