import { Box, Button, TextField } from '@mui/material';
import React, { useState, ChangeEvent, FC, useContext } from 'react';
import axios from 'axios';
import { isLoggedIn } from '../Authentication/cookieHandler';
import { SnackbarContext } from '../SnackBar/SnackbarContextProvider';

interface TweetProps {
	updateTweetsCallback?: () => void;
}

const Tweet: FC<TweetProps> = ({ updateTweetsCallback }) => {
	const [tweet, setTweet] = useState('');

	const {
		actions: { openSnackbar },
	} = useContext(SnackbarContext);

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
			openSnackbar('warning', 'Tweet is too long!');
		}
	};

	const handleSubmit = async () => {
		if (tweet === '') {
			openSnackbar('error', 'Tweet is empty!');
			return;
		}

		const user = isLoggedIn();

		if (user.isLoggedIn === false) {
			openSnackbar('error', 'You are not logged in!');
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
			.then(() => {
				if (updateTweetsCallback) {
					updateTweetsCallback();
					openSnackbar('success', 'Tweeted successfully!');
				} else {
					openSnackbar('success', 'Tweeted successfully! Refresh to see your tweet.');
				}
			})
			.catch((err) => {
				openSnackbar('error', 'Something went wrong!');
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
