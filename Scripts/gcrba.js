function SetActiveMenus(iconMenu, profileMenu) {
	try {
		if (iconMenu != "")
			$("#icon-bar-".concat(iconMenu)).addClass("active");
		if (profileMenu != undefined)
			$("#bakery-".concat(profileMenu)).addClass("active");
	}
	catch (Exception) { /* ignore errors here */ }
}

