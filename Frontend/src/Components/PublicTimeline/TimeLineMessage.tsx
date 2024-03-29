import React, { FC, Fragment, useState } from 'react';
import { Message } from '../../Types/Timeline';
import { Typography, ListItem, ListItemAvatar, Avatar, ListItemText, Skeleton } from '@mui/material';
import { Link } from 'react-router-dom';
import { isLoggedIn } from '../Authentication/cookieHandler';

interface props {
	message: Message;
	isSkeleton?: boolean;
}

const TimeLineMessage: FC<props> = ({ message, isSkeleton }) => {
	const [user] = useState(isLoggedIn());

	const style = {
		container: {
			border: '1px solid grey',
			borderRadius: '4px',
			margin: '4px 0',
			boxSizing: 'border-box',
			boxShadow: '0 0 3px 0px #888888aa',
			width: '27vw',
		},
		content: {
			display: 'flex',
			flexDirection: 'column',
		},
		text: {
			display: 'block',
			overflowWrap: 'break-word'
		},
		date: {
			fontSize: '0.7rem',
			color: 'grey',
			display: 'block',
		},
		skeletonBase: {
			margin: '0',
			padding: '0',
		},
	};

	const time = new Date(message.pubDate);

	if (isSkeleton) {
		return (
			<ListItem alignItems="flex-start" sx={style.container}>
				<ListItemAvatar>
					<Skeleton variant="circular" width={40} height={40} animation="wave" sx={style.skeletonBase} />
				</ListItemAvatar>
				<ListItemText
					primary={<Skeleton variant="text" width={100} sx={{ fontSize: '2rem', ...style.skeletonBase }} animation="wave" />}
					secondary={
						<Fragment>
							<Skeleton variant="text" width={200} height={20} animation="wave" sx={style.skeletonBase} />
							<Skeleton variant="text" width={100} height={20} sx={{ fontSize: '0.7rem', ...style.skeletonBase }} animation="wave" />
						</Fragment>
					}
				/>
			</ListItem>
		);
	}

	const linkPath = (
		(user.username === message.user) ?
		message.user + '/my-timeline/' : '/user/'+message.user);
	

	return (
		<ListItem alignItems="flex-start" sx={style.container}>
			<Link to={linkPath} reloadDocument={true}>
				<ListItemAvatar>
					<Avatar alt="Kusmar00" src="" />
				</ListItemAvatar>
			</Link>
			<ListItemText
				primary={
					<Link reloadDocument={true} to={linkPath} style={{ textDecoration: 'none', color: 'black' }}>
						<Typography variant="h6">{message.user}</Typography>
					</Link>
				}
				secondary={
					<Fragment>
						<Typography component="span" variant="body2" color="text.primary" sx={style.text}>
							{message.content}
						</Typography>
						<Typography component="span" variant="body2" color="text.primary" sx={style.date}>
							{time.toLocaleString()}
						</Typography>
					</Fragment>
				}
			/>
		</ListItem>
	);
};

export default TimeLineMessage;
