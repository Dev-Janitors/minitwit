import React, { FC, Fragment, useEffect, useState, ReactNode } from 'react';
import axios, { AxiosError } from 'axios';
import { Message } from '../../Types/Timeline';
import { IsLoading } from '../../Types/Global';
import TimeLineMessage from './TimeLineMessage';
import List from '@mui/material/List';
import { Typography, Box, Button } from '@mui/material';
import { useParams } from 'react-router-dom';
import { isLoggedIn } from '../Authentication/cookieHandler';

interface TimeLineContainerProps {
	children?: ReactNode;
}

const TimeLineContainer: FC<TimeLineContainerProps> = ({ children }) => {
	const { username } = useParams();
	const [timeline, setTimeline] = useState([] as Message[]);
	const [isLoading, setIsLoading] = useState({
		isLoading: true,
		error: null,
	} as IsLoading);

	const [user] = useState(isLoggedIn());
	const [userIsFollowed, setUserIsFollowed] = useState(false);

	useEffect(() => {
		const getTimeline = async () => {
			const baseUrl = `${process.env.REACT_APP_API_SERVER_URL}/msgs`;
			const options = username
				? {
						headers: {
							'access-control-allow-origin': `${process.env.REACT_APP_API_SERVER_URL}`,
						},
				  }
				: {
						headers: {
							'access-control-allow-origin': `${process.env.REACT_APP_API_SERVER_URL}`,
						},
				  };

			axios
				.get(username ? baseUrl + `/${username}` : baseUrl, options)
				.then((res) => {
					setTimeline(res.data);
					setIsLoading({ isLoading: false, error: null });
				})
				.catch((e) => {
					if (e instanceof AxiosError) {
						console.log(e);
						setIsLoading({ isLoading: false, error: e.message });
					} else {
						console.log(e);
						setIsLoading({ isLoading: false, error: 'Something went wrong!' });
					}
				});
		};
		getTimeline();

		if (user.isLoggedIn && username) {
			axios
				.get(`${process.env.REACT_APP_API_SERVER_URL}/fllws/${user.username}`, {
					headers: {
						'access-control-allow-origin': `${process.env.REACT_APP_API_SERVER_URL}`,
					},
				})
				.then((res) => {
					res.data.follows.forEach((follow: string) => {
						if (follow === username) {
							setUserIsFollowed(true);
						}
					});
				})
				.catch((e) => {
					console.log(e);
				});
		}
	}, []);

	const handleFollow = () => {
		console.log('Not implemented yet!');
	};

	const handleUnfollow = () => {
		console.log('Not implemented yet!');
	};

	if (isLoading.isLoading && isLoading.error === null) {
		{
			new Array(10).map((message, i) => {
				return (
					<Fragment key={i}>
						<TimeLineMessage message={{} as Message} isSkeleton={true} />
						{/* {i !== timeline.length - 1 && <Divider variant="inset" component="li" />} */}
					</Fragment>
				);
			});
		}
	} else if (isLoading.error !== null) {
		return <div>Error: {isLoading.error}</div>;
	}

	if (isLoading.isLoading && isLoading.error === null) {
		new Array(10).map((message, i) => {
			return (
				<Fragment key={i}>
					<TimeLineMessage message={{} as Message} isSkeleton={true} />
					{/* {i !== timeline.length - 1 && <Divider variant="inset" component="li" />} */}
				</Fragment>
			);
		});
	} else if (isLoading.error !== null) {
		return <div>Error: {isLoading.error}</div>;
	}
	if (timeline.length === 0) {
		return <Typography variant="h5">No messages</Typography>;
	}

	const userTimelineHeader = (
		<Box>
			<Typography variant="h3" sx={{ textTransform: 'capitalize' }}>
				{username}'s messages
			</Typography>
			{userIsFollowed ? (
				<>
					<Typography variant="h5">You are following this user</Typography>
					<Button onClick={handleUnfollow}>Unfollow</Button>
				</>
			) : (
				<Button onClick={handleFollow}>Follow</Button>
			)}
		</Box>
	);

	return (
		<>
			{username ? userTimelineHeader : <Typography variant="h3">Public timeline</Typography>}
			{children}
			<List sx={{ width: '100%', maxWidth: 360, bgcolor: 'background.paper' }}>
				{timeline.map((message, i) => {
					return (
						<Fragment key={i}>
							<TimeLineMessage message={message} />
							{/* {i !== timeline.length - 1 && <Divider variant="inset" component="li" />} */}
						</Fragment>
					);
				})}
			</List>
		</>
	);
};

export default TimeLineContainer;
