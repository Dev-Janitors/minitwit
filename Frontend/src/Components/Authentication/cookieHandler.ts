export function setCookie(name: string, value: string) {
	document.cookie = name + '=' + value + ';';
}

export function isLoggedIn() {
	const cookies = document.cookie.split(';');
	let username = '';
	cookies.forEach((cookie) => {
		if (cookie.includes('username')) {
			username = cookie.split('=')[1];
		}
	});

	if (username === '') {
		return { isLoggedIn: false, username: '' };
	}

	return { isLoggedIn: true, username };
}
