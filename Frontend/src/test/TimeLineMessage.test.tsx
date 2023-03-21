import React from 'react';
import { screen } from '@testing-library/react';
import { createRoot } from 'react-dom/client';
import { act } from '@testing-library/react';
import TimeLineMessage from '../Components/PublicTimeline/TimeLineMessage';
import { BrowserRouter } from 'react-router-dom';

let container = {} as Element | DocumentFragment;

beforeEach(() => {
	container = document.createElement('div');
	document.body.appendChild(container);
});

afterEach(() => {
	document.body.removeChild(container as Element);
	container = {} as Element | DocumentFragment;
});

describe('TimeLineMessage', () => {
	const mockMesage = { content: 'Best Message!', user: 'TestUser', pubDate: new Date().getMilliseconds() };
	it('should render', () => {
		act(() => {
			createRoot(container).render(
				<BrowserRouter>
					<TimeLineMessage message={mockMesage} />
				</BrowserRouter>
			);
		});
	});
});
