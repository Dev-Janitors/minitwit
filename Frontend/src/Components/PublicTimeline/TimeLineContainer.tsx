import React, { FC, Fragment, useEffect, useState, ReactNode } from 'react';
import { Message } from '../../Types/Timeline';
import TimeLineMessage from './TimeLineMessage';
import { Typography, Box, Button } from '@mui/material';
import InfiniteScroll from 'react-infinite-scroll-component';

interface TimeLineContainerProps {
	children?: ReactNode;
	messages: Message[];
	getNextMessages: () => void;
	isLoading: any;
	hasMore: boolean;
	title?: string;
}

const TimeLineContainer: FC<TimeLineContainerProps> = ({ children, messages, getNextMessages, isLoading, hasMore, title }) => {

	if (isLoading.isLoading && isLoading.error === null) {
		new Array(10).map((message, i) => {
			return (
				<Fragment key={i}>
					<TimeLineMessage message={{} as Message} isSkeleton={true} />
				</Fragment>
			);
		});
	} else if (isLoading.error !== null) {
		return <div>Error: {isLoading.error}</div>;
	}

	if (isLoading.isLoading && isLoading.error === null) {
		new Array(10).map((message, i) => {
			return (
				<Fragment key={i}>
					<TimeLineMessage message={{} as Message} isSkeleton={true} />
				</Fragment>
			);
		});
	} else if (isLoading.error !== null) {
		return <div>Error: {isLoading.error}</div>;
	}

	if (!isLoading.isLoading && messages.length === 0) {
		return (
			<>
				<Typography variant="h5">No messages</Typography>
			</>
		);
	}

	return (
		<>
			<Typography variant='h4' sx={{marginBottom: '25px'}}>{title}</Typography>
			<InfiniteScroll
				dataLength={messages.length} //This is important field to render the next data
				next={getNextMessages}
				hasMore={hasMore}
				loader={<Typography variant="h6">Loading...</Typography>}
				endMessage={
					<p style={{ textAlign: 'center' }}>
						<b>Yay! You have seen it all</b>
					</p>
				}
			>
				{messages.map((message, i) => {
					return <TimeLineMessage message={message} key={i} />;
				})}
			</InfiniteScroll>
		</>
	);
};

export default TimeLineContainer;
