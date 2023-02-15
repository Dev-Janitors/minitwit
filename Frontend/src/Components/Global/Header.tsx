import React from 'react';
import { Box, Typography, Link } from '@mui/material';

const Header = () => {
	const style = {
		header: {
			display: 'flex',
			flexDirection: 'row',
			alignItems: 'center',
			justifyContent: 'space-between',
			height: '10vh',
			minHeight: '100px',
			backgroundColor: 'primary.main',
			width: '100%',
			color: 'primary.contrastText',
		},
		headerTitle: {
			marginLeft: '10px',
			fontSize: '3rem',
			fontWeight: '500',
		},
		loginContainer: {
			display: 'flex',
			flexDirection: 'row',
			alignItems: 'center',
			justifyContent: 'center',
			flexWrap: 'wrap',
			width: '20%',
			minWidth: '100px',
		},
		button: {
			margin: '5px',
			flex: 1,
			maxWidth: '200px',
			color: 'primary.contrastText',
			borderColor: 'primary.contrastText',
			fontSize: '1rem',
		},
	};
	return (
		<Box sx={style.header}>
			<Typography variant="h2" style={style.headerTitle}>
				MiniTwit
			</Typography>
			<Box sx={style.loginContainer}>
				<Link component="button" href="#" sx={style.button}>
					Login
				</Link>
				<Link component="button" variant="body2" sx={style.button}>
					Register
				</Link>
			</Box>
		</Box>
	);
};

export default Header;
