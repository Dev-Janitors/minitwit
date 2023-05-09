import React, { FC, useEffect, useState } from 'react';
import { Box, Typography } from '@mui/material';
import Header from '../Global/Header/Header';
import { Message } from '../../Types/Timeline';
import TimeLineContainer from '../PublicTimeline/TimeLineContainer';
import { isLoggedIn, setCookie } from '../Authentication/cookieHandler';
import { IsLoading } from '../../Types/Global';
import axios, { AxiosError } from 'axios';
import Tweet from '../Tweet/Tweet';

const PublicTimelinePage: FC = () => {
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

	const [user, setUser] = useState(isLoggedIn());

	const getTimeline = async (startIndex?: number, endIndex?: number) => {
		const baseUrl = `http://${window.location.hostname}:2222/msgs`;
		const queryParams = startIndex !== undefined && endIndex !== undefined ? `?startIndex=${startIndex}&endIndex=${endIndex}` : '';

		const options = {
			headers: {
				'access-control-allow-origin': `${window.location.hostname}:2222`,
			},
		};

		const fullUrl = baseUrl + queryParams;

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

		const baseUrl = `http://${window.location.hostname}:2222/msgs`;

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
			<Header setPageUser={setUser} />
			<Typography variant="h4">Public Timelinesdkajfdæsaj</Typography>
			{!isLoading.isLoading && user.isLoggedIn && <Tweet updateTweetsCallback={getTimeline} />}
			<TimeLineContainer title="Public Timeline" messages={timeline} getNextMessages={fetchMoreData} hasMore={hasMore} isLoading={isLoading} />
		</Box>
	);
};

export default PublicTimelinePage;
