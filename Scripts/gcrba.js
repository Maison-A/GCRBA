function SetActiveMenus(iconMenu, profileMenu) {
	try {
		if (iconMenu != "")
			$("#icon-bar-".concat(iconMenu)).addClass("active");
		if (profileMenu != undefined)
			$("#profile-".concat(profileMenu)).addClass("active");
	}
	catch (Exception) { /* ignore errors here */ }
}


//function NewMemberCheckbox() {
//	if ($('#IsChecked').is(":checked"))
		
//	else
//		$("#MyTxt").hide();
//}