// hide until toggled
// -----------------------------------------------
$('.edit-company-nav-options').hide();
$('#add-contact-area').hide();
$('#edit-contact-area').hide();
$('#remove-contact-area').hide();
$('#manage-contacts-by-location').hide();
$('#manage-contacts').hide();
// -----------------------------------------------

// toggle hide/show 
// -----------------------------------------------
$('#add-new-contact').click(function () {
	$('#add-contact-area').toggle();
});

$('#edit-existing-contact').click(function () {
	$('#edit-contact-area').toggle();
});

$('#remove-contacts').click(function () {
	$('#remove-contact-area').toggle();
});

$('#manage-by-location').click(function () {
	$('#manage-contacts-by-location').toggle();
});
// -----------------------------------------------

// toggle hide/show of company edit options 
$('#edit-companies-list').change(hideShowCompanyOptions);

// send form input to controller method when clicked 
$('#add-new-contact-submit').click(postNewContact);
$('#edit-contact-submit').click(postEditedContact);

function hideShowCompanyOptions() {

	// is company selected?
	if ($('#edit-companies-list').val() > 0) {

		// yes, show nav options 
		$('.edit-company-nav-options').show();

	} else {

		// no, so don't show nav options 
		$('.edit-company-nav-options').hide();

	}
}

function postNewContact() {

	// variable to hold message for success/error
	var statusMessage = '';

	// /controller/method 
	var url = '/AdminPortal/SubmitNewContact';

	// form input 
	var firstName = $('#new-first-name-input').val();
	var lastName = $('#new-last-name-input').val(); 

	// validate email and phone format 
	document.getElementById('new-email-input').reportValidity();
	document.getElementById('new-phone-input').reportValidity();

	var phone = $('#new-phone-input').val();
	var email = $('#new-email-input').val();
	var locationID = $('#new-contact-location-list').val();
	var typeID = $('#new-contact-type-list').val();

	// send form input to controller method as post call 
	// get message back -- success or failure
	$.post(url, { FirstName: firstName, LastName: lastName, Phone: phone, Email: email, LocationID: locationID, TypeID: typeID }, function (data) {

		if (data == 'RequiredFieldsMissing') {
			statusMessage = 'Name, location, and contact type may not be blank. Please try again.';
		}

		if (data == "PhoneFormatIssue") {
			statusMessage = 'Phone number must be 10 digits using numeric input only. Please try again. (Example: 5135551234)';
		}

		if (data == "DuplicateName") {
			statusMessage = 'This contact already exists.';
			resetNewContactFields();
		}

		if (data == 'InsertSuccessful') {
			statusMessage = 'Contact successfully added.';
			resetNewContactFields();
		}

		if (data == 'Unknown') {
			statusMessage = 'There was an error when processing your request. Please try again later.';
		}

		displayStatusMessage('submission-message', statusMessage);

		hideStatusMessage('submission-message');
	});
}

function postEditedContact() {
	// variable to hold success/error message
	var statusMessage = '';

	// controller/method called
	var url = '/AdminPortal/UpdateContact';

	// form input 
	var contactID = $('#edit-contacts-list').val();
	var firstName = $('#first-name-input').val();
	var lastName = $('#last-name-input').val();

	// validate email and phone input 
	document.getElementById('phone-input').reportValidity();
	document.getElementById('email-input').reportValidity();

	var phone = $('#phone-input').val();
	var email = $('#email-input').val();

	$.post(url, { FirstName: firstName, LastName: lastName, Phone: phone, Email: email, ContactID: contactID }, function (data) {

		if (data == 'PhoneFormatIssue') {
			statusMessage = 'Phone number must be 10 digits using numeric input only. Please try again. (Example: 5135551234)';
		}

		if (data == "UpdateSuccessful") {
			statusMessage = 'Contact successfully updated.';
		}

		if (data == 'Unknown') {
			statusMessage = 'There was an error when processing your request. Please try again later.';
		}

		if (data == 'NoType') {
			statusMessage = 'No changes submitted.'
		}

		displayStatusMessage('edit-submission-message', statusMessage);
		hideStatusMessage('edit-submission-message');
	})

	resetEditedContactFields();
}

$('#edit-contacts-list').change(function () {
	$.get('/AdminPortal/ContactInfo/' + $('#edit-contacts-list').val(), function (data) {
		$.each(data, function (index, value) {

			if (index == 'FirstName') {
				var $firstName = $('#first-name-input');
				$firstName.val(value);
			}

			if (index == 'LastName') {
				var $lastName = $('#last-name-input');
				$lastName.val(value);
			}

			if (index == 'Phone') {
				var $phone = $('#phone-input');
				$phone.val(value);
			}

			if (index == 'Email') {
				var $email = $('#email-input');
				$email.val(value);
			}
		});
	});
});

function displayStatusMessage(messageLocation, statusMessage) {
	$('#' + messageLocation).show();
	$('#' + messageLocation).html(statusMessage);
}

function resetNewContactFields() {
	// reset input fields 
	$('#new-name-input').val('');
	$('#new-phone-input').val('');
	$('#new-email-input').val('');

	$('#new-contact-type-list').find('#default-type').remove();
	$('#new-contact-type-list').prepend('<option disabled selected>Choose Contact Type for Selected Location</option>');

	$('#new-contact-location-list').find('#default-location').remove();
	$('#new-contact-location-list').prepend('<option disabled selected>Select Location</option>');
}

function resetEditedContactFields() {
	$('#first-name-input').val('');
	$('#last-name-input').val('');
	$('#phone-input').val('');
	$('#email-input').val('');

	// remove current options from select list 
	$('#edit-contacts-list').find('option').remove();
	$('#edit-contacts-list').prepend('<option disabled selected>Contacts</option>');

	$.get('/AdminPortal/GetContactsByCompany/' + $('#edit-contacts-list').val(), function (data) {
		$.each(data, function (index, value) {

			// add updated contacts to select list
			$('#edit-contacts-list').append('<option value="' + value.ContactPersonID + '">' + value.FullName + '</option>');

		})
	})

}

function hideStatusMessage(messageLocation) {

	// hide message after 5 seconds
	setTimeout('$("#' + messageLocation + '").hide()', 5000);

}

function SetActiveMenus(iconMenu, profileMenu) {
	try {
		if (iconMenu != "")
			$("#icon-bar-".concat(iconMenu)).addClass("active");
		if (profileMenu != undefined)
			$("#bakery-".concat(profileMenu)).addClass("active");
	}
	catch (Exception) { /* ignore errors here */ }
}

function deleteImageAjax(deleteType, uid, id) {
	try {
		var ajaxData = { //json structure
			UID: uid,
			ID: id
		};

		var strURL;
		if (deleteType == "location")
			strURL = "../Photo/DeleteImage";
		else //event
			strURL = "../../Photo/DeleteEventImage";

		$.ajax({
			type: "POST",
			url: strURL,
			data: ajaxData,
			success: function (returnData) {
				if (returnData.Status == 1) {
					$("#image-".concat(id)).hide();
				}
				else {
					alert('Unable to remove image.');
				}
			},
			error: function (xhr) {
				debugger;
			}
		});
	}
	catch (err) {
		showError(err);
	}
}