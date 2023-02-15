import React, { FC } from 'react';
import { Message } from '../../Types/Timeline';
import { Box, Typography } from '@mui/material';

interface props {
	message: Message;
}

const TimeLineMessage: FC<props> = ({ message }) => {
	const style = {
		container: {
			display: 'flex',
			flexDirection: 'column',
			alignItems: 'center',
			justifyContent: 'center',
			borderRadius: '10px',
			backgroundColor: 'grey',
			width: '70%',
			margin: '4px',
			padding: '4px',
			boxSizing: 'border-box',
		},
	};

	const date = new Date(message.pubDate);

	return (
		<Box sx={style.container}>
			<Box>
				<Typography>Username</Typography>
			</Box>
			<Typography>{message.text}</Typography>
			<Typography>{date.toLocaleString()}</Typography>
		</Box>
	);
};

export default TimeLineMessage;
