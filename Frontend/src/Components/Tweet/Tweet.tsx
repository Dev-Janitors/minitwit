import { Box, Button, TextField } from '@mui/material';
import React, { useState, ChangeEvent, FC, useContext, KeyboardEvent } from 'react';
import axios from 'axios';
import { isLoggedIn } from '../Authentication/cookieHandler';
import { SnackbarContext } from '../SnackBar/SnackbarContextProvider';
import SendIcon from '@mui/icons-material/Send';

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
			marginTop: '10px',
			marginBottom: '20px',
			width: '30vw',
			height: '175px',
			paddingLeft: '50px',
			paddingRight: '50px',
			border: '1px solid grey',
			borderRadius: '5px',
			display: 'flex',
			flexDirection: 'column',
			justifyContent: 'center',
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

		const baseUrl = `http://${window.location.hostname}:2222/msgs/${user.username}?latest=101`;
		const options = {
			headers: {
				'access-control-allow-origin': `${window.location.hostname}:2222`,
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
			<TextField className="textField" label="Tweet" multiline maxRows={4} placeholder="What's on your mind?" variant="filled" onChange={handleChange} value={tweet} />
			<Button className="submit" onClick={handleSubmit} variant="contained" endIcon={<SendIcon />}>
				publish
			</Button>
		</Box>
	);
};

export default Tweet;
