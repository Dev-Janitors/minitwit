export interface UserData {
	isLoggedIn: boolean;
	username: string;
}

export function setCookie(name: string, value: string, expiration: number = 24) {
	const date = new Date();
	date.setTime(date.getTime() + expiration * 60 * 60 * 1000);
	document.cookie = `${name}=${value};expires=${date.toUTCString()};path=/`;
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

export function logout() {
	document.cookie = 'username=;expires=Thu, 01 Jan 1970 00:00:00 UTC;path=/;';
}
