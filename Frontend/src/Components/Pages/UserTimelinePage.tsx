import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import axios, { AxiosError } from 'axios';
import { Box, Button, Typography } from '@mui/material';
import { isLoggedIn } from '../Authentication/cookieHandler';
import { IsLoading } from '../../Types/Global';
import { Message } from '../../Types/Timeline';
import TimeLineContainer from '../PublicTimeline/TimeLineContainer';
import Header from '../Global/Header/Header';

const UserTimelinePage = () => {
	const style = {
		container: {
			display: 'flex',
			flexDirection: 'column',
			alignItems: 'center',
			justifyContent: 'center',
		},
	};

	const [timeline, setTimeline] = useState([] as Message[]);

	const [hasMore, setHasMore] = useState(true);

	const [isLoading, setIsLoading] = useState({
		isLoading: true,
		error: null,
	} as IsLoading);

	const [userIsFollowed, setUserIsFollowed] = useState(false);

	const [user] = useState(isLoggedIn());

	const { username } = useParams();

	const handleFollow = () => {
		if (!user.isLoggedIn) {
			return;
		}
		const baseUrl = `http://${window.location.hostname}:2222/fllws/${user.username}`;
		const options = {
			headers: {
				'access-control-allow-origin': `${window.location.hostname}:2222`,
			},
		};
		axios
			.post(baseUrl, { follow: username }, options)
			.then((res) => {
				if (res.status === 204) {
					setUserIsFollowed(true);
				} else {
					console.log(res);
				}
			})
			.catch((e) => {
				console.log(e);
			});
	};

	const handleUnfollow = () => {
		if (!user.isLoggedIn) {
			return;
		}
		const baseUrl = `http://${window.location.hostname}:2222/fllws/${user.username}`;
		const options = {
			headers: {
				'access-control-allow-origin': `${window.location.hostname}:2222`,
			},
		};
		axios
			.post(baseUrl, { unfollow: username }, options)
			.then((res) => {
				if (res.status === 204) {
					setUserIsFollowed(false);
				} else {
					console.log(res);
				}
			})
			.catch((e) => {
				console.log(e);
			});
	};

	const getMessages = async (startIndex?: number, endIndex?: number) => {
		const baseUrl = `http://${window.location.hostname}:2222/msgs/${username}`;
		const queryParams = startIndex !== undefined && endIndex !== undefined ? `?startIndex=${startIndex}&endIndex=${endIndex}` : '';
		const fullUrl = baseUrl + queryParams;

		const options = {
			headers: {
				'access-control-allow-origin': `${window.location.hostname}:2222`,
			},
		};

		axios
			.get(fullUrl, options)
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

	const fetchMoreData = () => {
		const startIndex = timeline.length;
		const endIndex = startIndex + 40;

		const baseUrl = `http://${window.location.hostname}:2222/msgs/${username}`;

		const queryParams = startIndex !== undefined && endIndex !== undefined ? `?startIndex=${startIndex}&endIndex=${endIndex}` : '';

		const fullUrl = baseUrl + queryParams;

		const options = {
			headers: {
				'access-control-allow-origin': `${window.location.hostname}:2222`,
			},
		};

		axios
			.get(fullUrl, options)
			.then((res) => {
				console.log(res.data);
				if (res.data.length === 0) {
					setHasMore(false);
				}
				setTimeline([...timeline, ...res.data]);
			})
			.catch((e) => {
				if (e instanceof AxiosError) {
					console.log(e);
				} else {
					console.log(e);
				}
			});
	};

	useEffect(() => {
		// Todo: If username === user.username go to my-timeline page

		getMessages(0, 40);
		if (user.isLoggedIn) {
			axios
				.get(`http://${window.location.hostname}:2222/fllws/${user.username}`, {
					headers: {
						'access-control-allow-origin': `${window.location.hostname}:2222`,
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

	return (
		<Box sx={style.container}>
			<Header />
			<Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center' }}>
				{user.isLoggedIn ? (
					userIsFollowed ? (
						<Button onClick={handleUnfollow}>Unfollow {username}</Button>
					) : user.username !== username ? (
						<Button onClick={handleFollow}>Follow {username}</Button>
					) : (
						<></>
					)
				) : (
					<></>
				)}
			</Box>
			<TimeLineContainer messages={timeline} getNextMessages={fetchMoreData} hasMore={hasMore} isLoading={isLoading} title={username + "'s Tweets"} />
		</Box>
	);
};

export default UserTimelinePage;
