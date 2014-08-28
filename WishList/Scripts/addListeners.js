﻿(function () {
    'use strict';

    $('button.btn').on('click', function () {
        alert("Changed");
        callController($(this).attr('id'));
    });

    function callController(userId) {
        $.ajax({
            url: '../Admin/SwitchRole',
            dataType: 'html',
            traditional: true,
            type: 'GET',
            data: { userId: userId, roleId: $('.' + userId).val() }
        });
    }
}());