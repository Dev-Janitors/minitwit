import { Box, Button, TextField } from '@mui/material';
import React, { useState, ChangeEvent } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import axios from 'axios';
import { isLoggedIn } from '../Authentication/cookieHandler';

const Tweet = () => {
	const [tweet, setTweet] = useState('');

	const style = {
		container: {
			display: 'flex',
			flexDirection: 'row',
			justifyContent: 'space-between',
			width: '300px',
		},
	};

	// const { getAccessTokenSilently, user } = useAuth0();

	const handleChange = (event: ChangeEvent<HTMLInputElement>) => {
		if (event.target.value.length <= 280) {
			setTweet(event.target.value);
		} else {
			console.log('tweet is too long');
		}
	};

	const handleSubmit = async () => {
		// if (user === undefined) {
		// 	console.log('not logged in');
		// 	return;
		// }
		//TODO: we have to know the username somehow, maybe just save it on the frontend?
		const user = isLoggedIn();

		if (user.isLoggedIn === false) {
			console.log('not logged in');
			return;
		}

		const baseUrl = `${process.env.REACT_APP_API_SERVER_URL}/msgs/${user.username}?latest=101`;
		const options = {
			headers: {
				'access-control-allow-origin': `${process.env.REACT_APP_API_SERVER_URL}`,
			},
			method: 'POST',
		};

		axios
			.post(baseUrl, { content: tweet }, options)
			.then((res) => {
				console.log(res);
			})
			.then((err) => {
				console.log(err);
			});
	};

	return (
		<Box sx={style.container}>
			<TextField label="Tweet" multiline rows={4} placeholder="Default Value" variant="filled" onChange={handleChange} value={tweet} />
			<Button onClick={handleSubmit} variant="contained">
				Tweet
			</Button>
		</Box>
	);
};

export default Tweet;
