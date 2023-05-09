import { Box } from '@mui/material';
import axios, { AxiosError } from 'axios';
import React, { useEffect, useState } from 'react';
import { IsLoading } from '../../Types/Global';
import { Message } from '../../Types/Timeline';
import { isLoggedIn } from '../Authentication/cookieHandler';
import Header from '../Global/Header/Header';
import TimeLineContainer from '../PublicTimeline/TimeLineContainer';
import Tweet from '../Tweet/Tweet';

const MyTimelinePage = () => {
	const style = {
		container: {
			display: 'flex',
			flexDirection: 'column',
			alignItems: 'center',
			justifyContent: 'center',
		},
	};

	const [user] = useState(isLoggedIn());

	const [timeline, setTimeline] = useState([] as Message[]);

	const [hasMore, setHasMore] = useState(true);

	const [isLoading, setIsLoading] = useState({
		isLoading: true,
		error: null,
	} as IsLoading);

	const getTimeline = async (startIndex?: number, endIndex?: number) => {
		const baseUrl = `${window.location.hostname}:2222/my-timeline/${user.username}`;

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

	// TODO: Make this endpoint work like the others
	const fetchMoreData = () => {
		const startIndex = timeline.length;
		const endIndex = startIndex + 40;

		const baseUrl = `${window.location.hostname}:2222/my-timeline/${user.username}`;

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
		getTimeline(0, 40);
	}, []);

	return (
		<Box sx={style.container}>
			<Header />
			{!isLoading.isLoading && user.isLoggedIn && <Tweet updateTweetsCallback={getTimeline} />}
			<TimeLineContainer messages={timeline} getNextMessages={fetchMoreData} hasMore={hasMore} title="Your Timeline" isLoading={isLoading} />
		</Box>
	);
};

export default MyTimelinePage;
