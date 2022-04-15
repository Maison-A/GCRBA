function SetActiveMenus(iconMenu, profileMenu) {
	try {
		if (iconMenu != "")
			$("#icon-bar-".concat(iconMenu)).addClass("active");
		if (profileMenu != undefined)
			$("#profile-".concat(profileMenu)).addClass("active");
	}
	catch (Exception) { /* ignore errors here */ }
}

// when document is ready 
$(document).ready(function () {

	// set as initially hidden 
	$("#addNewLocationInput").hide();

	// toggle between hidden and visible 
	$("#addNewLocation").click(function () {
		$("#addNewLocationInput").toggle();
	});

});