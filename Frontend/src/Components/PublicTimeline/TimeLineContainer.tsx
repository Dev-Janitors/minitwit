import React, { FC, Fragment, useEffect, useState } from 'react';
import axios, { AxiosError } from 'axios';
import { Message } from '../../Types/Timeline';
import { IsLoading } from '../../Types/Global';
import TimeLineMessage from './TimeLineMessage';
import List from '@mui/material/List';
import { Typography } from '@mui/material';
import { useParams } from 'react-router-dom';
import { useAuth0 } from '@auth0/auth0-react';

const TimeLineContainer: FC = () => {
	const { username } = useParams();
	const { getAccessTokenSilently } = useAuth0();
	const [timeline, setTimeline] = useState([] as Message[]);
	const [isLoading, setIsLoading] = useState({
		isLoading: true,
		error: null,
	} as IsLoading);

	useEffect(() => {
		const getTimeline = async () => {
			const baseUrl = `${process.env.REACT_APP_API_SERVER_URL}/msgs`;
			const options = username
				? {
						headers: {
							'access-control-allow-origin': `${process.env.REACT_APP_API_SERVER_URL}`,
							Authorization: `Bearer ${await getAccessTokenSilently()}`,
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
	}, []);

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

	return (
		<>
			<Typography variant="h3" sx={{ textTransform: 'capitalize' }}>
				{username ? `${username}'s messages` : 'Public timeline'}
			</Typography>
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
