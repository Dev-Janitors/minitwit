import React, { FC, useState, useContext } from 'react';
import { Box, Input, Typography, Button } from '@mui/material';
import axios from 'axios';
import { UserData, isLoggedIn, setCookie } from './cookieHandler';
import CloseIcon from '@mui/icons-material/Close';
import { SnackbarContext } from '../SnackBar/SnackbarContextProvider';

interface user {
	isLoggedIn: boolean;
	username: string;
}

interface SelfAuthenticationProps {
	modalOpen: boolean;
	handleMenuClose: () => void;
	loginCallback?: (userData: UserData) => void;
	setPageUser?: (user: user) => void;
}

const SelfAuthentication: FC<SelfAuthenticationProps> = ({ modalOpen, handleMenuClose, loginCallback, setPageUser }) => {
	const [login, setLogin] = useState('login');

	const [username, setUsername] = useState('');
	const [email, setEmail] = useState('');

	const {
		actions: { openSnackbar },
	} = useContext(SnackbarContext);

	const style = {
		containerOpen: {
			position: 'absolute',
			top: '10%',
			right: '4px',
			display: 'flex',
			flexDirection: 'column',
			justifyContent: 'center',
			alignItems: 'center',
			transition: 'all 0.5s ease-out',
			zIndex: 100,
			backgroundColor: 'white',
			padding: '10px',
			boxSizing: 'border-box',
			border: '1px solid black',
			backdropFilter: 'blur(5px)',
		},
		containerClosed: {
			position: 'absolute',
			top: '0',
			left: '50%',
			transform: 'translateX(-50%)',
			display: 'none',
			transition: 'all 0.5s ease-out',
			zIndex: 100,
		},
		button: {
			margin: '10px',
		},
		activeButton: {
			margin: '10px',
			backgroundColor: 'primary.main',
		},
		disabledButton: {
			margin: '10px',
			backgroundColor: 'orange',
		},
	};

	const changeScreen = (to: 'login' | 'register') => {
		setLogin(to);
		setUsername('');
		setEmail('');
	};

	const handleLogin = () => {
		const baseUrl = `http://${window.location.hostname}:2222/login`;
		const options = {
			headers: {
				'access-control-allow-origin': `http://${window.location.hostname}:2222`,
			},
			method: 'POST',
		};

		axios
			.post(baseUrl, { Username: username, Email: email }, options)
			.then((res) => {
				setCookie('username', res.data.username);
				if (loginCallback) {
					loginCallback(isLoggedIn());
					openSnackbar('success', 'Logged in successfully!');
				} else {
					openSnackbar('success', 'Refresh the page to see your profile!');
				}
				handleMenuClose();
			})
			.catch((err) => {
				openSnackbar('error', 'Login failed!');
				console.log(err);
			});

		setPageUser && setPageUser({ isLoggedIn: true, username: username });
	};

	const handleRegister = () => {
		const baseUrl = `http://${window.location.hostname}:2222/register`;
		const options = {
			headers: {
				'access-control-allow-origin': `http://${window.location.hostname}:2222`,
			},
			method: 'POST',
		};

		axios
			.post(baseUrl, { Username: username, Email: email }, options)
			.then((res) => {
				console.log(res);
				setCookie('username', username);
				if (loginCallback) {
					loginCallback(isLoggedIn());
					openSnackbar('success', 'Registered successfully!');
				} else {
					openSnackbar('success', 'Refresh the page to see your profile!');
				}
				handleMenuClose();
			})
			.catch((err) => {
				openSnackbar('error', 'Registration failed!');
				console.log(err);
			});

		setPageUser && setPageUser({ isLoggedIn: true, username: username });
	};

	return (
		<Box sx={modalOpen ? style.containerOpen : style.containerClosed}>
			<CloseIcon onClick={handleMenuClose} />
			<Box>
				<Button onClick={() => changeScreen('login')} variant="contained" sx={login === 'login' ? style.activeButton : style.disabledButton}>
					Login
				</Button>
				<Button onClick={() => changeScreen('register')} variant="contained" sx={login === 'login' ? style.disabledButton : style.activeButton}>
					Register
				</Button>
				<Box>
					<Typography variant="h5">Username</Typography>
					<Input type="text" onChange={(e) => setUsername(e.target.value)} value={username} />
					<Typography variant="h5">Email</Typography>
					<Input type="email" onChange={(e) => setEmail(e.target.value)} value={email} />
				</Box>
				<Button variant="contained" onClick={login === 'login' ? handleLogin : handleRegister}>
					{login === 'login' ? 'Login' : 'Register'}
				</Button>
			</Box>
		</Box>
	);
};

export default SelfAuthentication;
