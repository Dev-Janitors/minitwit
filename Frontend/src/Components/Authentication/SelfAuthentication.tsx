import React, { useState } from 'react';
import { Box, Input, Typography, Button } from '@mui/material';
import axios from 'axios';
import { setCookie } from './cookieHandler';

const SelfAuthentication = () => {
	const [login, setLogin] = useState('login');

	const [username, setUsername] = useState('');
	const [email, setEmail] = useState('');

	const style = {
		container: {
			display: 'flex',
			flexDirection: 'column',
			justifyContent: 'center',
			alignItems: 'center',
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
			backgroundColor: 'secondary.main',
		},
	};

	const changeScreen = (to: 'login' | 'register') => {
		setLogin(to);
		setUsername('');
		setEmail('');
	};

	const handleLogin = () => {
		const baseUrl = `${process.env.REACT_APP_API_SERVER_URL}/login`;
		const options = {
			headers: {
				'access-control-allow-origin': `${process.env.REACT_APP_API_SERVER_URL}`,
			},
			method: 'POST',
		};

		axios
			.post(baseUrl, { Username: username, Email: email }, options)
			.then((res) => {
				setCookie('username', res.data.username);
			})
			.then((err) => {
				console.log(err);
			});
	};

	const handleRegister = () => {
		const baseUrl = `${process.env.REACT_APP_API_SERVER_URL}/register`;
		const options = {
			headers: {
				'access-control-allow-origin': `${process.env.REACT_APP_API_SERVER_URL}`,
			},
			method: 'POST',
		};

		axios
			.post(baseUrl, { Username: username, Email: email }, options)
			.then((res) => {
				console.log(res);
			})
			.then((err) => {
				console.log(err);
			});
	};

	return (
		<Box sx={style.container}>
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
