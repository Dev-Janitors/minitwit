import React, { useState, MouseEvent, useContext, FC } from 'react';
import { styled } from '@mui/material/styles';
import { AppBar, Box, Toolbar, IconButton, Typography, InputBase, MenuItem, Menu } from '@mui/material';
import AccountCircle from '@mui/icons-material/AccountCircle';
import LogoutIcon from '@mui/icons-material/Logout';
import { Link } from 'react-router-dom';
import SelfAuthentication from '../../Authentication/SelfAuthentication';
import { isLoggedIn, logout } from '../../Authentication/cookieHandler';
import { SnackbarContext } from '../../SnackBar/SnackbarContextProvider';

interface user {
	isLoggedIn: boolean;
	username: string;
}

interface headerProps {
	setPageUser?: (user: user) => void;
}

const Header: FC<headerProps> = ({ setPageUser }) => {
	const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
	// const { isLoading, isAuthenticated, loginWithRedirect, logout, user } = useAuth0();
	const [mobileMoreAnchorEl, setMobileMoreAnchorEl] = useState<null | HTMLElement>(null);

	const [user, setUser] = useState(isLoggedIn());

	const isMenuOpen = Boolean(anchorEl);
	const isMobileMenuOpen = Boolean(mobileMoreAnchorEl);

	const {
		actions: { openSnackbar },
	} = useContext(SnackbarContext);

	const handleProfileMenuOpen = (event: MouseEvent<HTMLElement>) => {
		setAnchorEl(event.currentTarget);
	};

	const handleMobileMenuClose = () => {
		setMobileMoreAnchorEl(null);
	};

	const handleMenuClose = () => {
		setAnchorEl(null);
		handleMobileMenuClose();
	};

	const handleMobileMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
		setMobileMoreAnchorEl(event.currentTarget);
	};

	const handleLogout = () => {
		logout();
		if (isMenuOpen) handleMenuClose();
		if (isMobileMenuOpen) handleMobileMenuClose();
		setUser({ isLoggedIn: false, username: '' });
		setPageUser && setPageUser({ isLoggedIn: false, username: '' } as user);
		openSnackbar('success', 'You have been logged out');
	};

	const menuId = 'primary-search-account-menu';
	const renderAuthMenu = (
		<Menu
			anchorEl={anchorEl}
			anchorOrigin={{
				vertical: 'top',
				horizontal: 'right',
			}}
			id={menuId}
			keepMounted
			transformOrigin={{
				vertical: 'top',
				horizontal: 'right',
			}}
			open={isMenuOpen}
			onClose={handleMenuClose}
		>
			<Link reloadDocument={true} to={'/' + user.username + '/my-timeline'} style={{ textDecoration: 'none', color: 'black' }}>
				<MenuItem onClick={handleMenuClose}>My Timeline</MenuItem>
			</Link>
			<MenuItem onClick={handleLogout}>Logout</MenuItem>
		</Menu>
	);

	// const renderNoAuthMenu = (
	// 	<Menu
	// 		anchorEl={anchorEl}
	// 		anchorOrigin={{
	// 			vertical: 'top',
	// 			horizontal: 'right',
	// 		}}
	// 		id={menuId}
	// 		keepMounted
	// 		transformOrigin={{
	// 			vertical: 'top',
	// 			horizontal: 'right',
	// 		}}
	// 		open={isMenuOpen}
	// 		onClose={handleMenuClose}
	// 	>
	// 		<MenuItem onClick={() => loginWithRedirect()}>Login</MenuItem>
	// 		<MenuItem
	// 			onClick={() =>
	// 				loginWithRedirect({
	// 					authorizationParams: {
	// 						screen_hint: 'signup',
	// 					},
	// 				})
	// 			}
	// 		>
	// 			Signup
	// 		</MenuItem>
	// 	</Menu>
	// );

	const renderNoAuthMenu = <SelfAuthentication setPageUser={setPageUser} modalOpen={isMenuOpen} handleMenuClose={handleMenuClose} loginCallback={setUser} />;

	const mobileMenuId = 'primary-search-account-menu-mobile';
	const renderMobileAuthMenu = (
		<Menu
			anchorEl={mobileMoreAnchorEl}
			anchorOrigin={{
				vertical: 'top',
				horizontal: 'right',
			}}
			id={mobileMenuId}
			keepMounted
			transformOrigin={{
				vertical: 'top',
				horizontal: 'right',
			}}
			open={isMobileMenuOpen}
			onClose={handleMobileMenuClose}
		>
			<MenuItem onClick={handleProfileMenuOpen}>
				<IconButton size="large" aria-label="account of current user" aria-controls="primary-search-account-menu" aria-haspopup="true" color="inherit">
					<AccountCircle />
				</IconButton>
				<p>Profile</p>
			</MenuItem>
			<MenuItem onClick={handleLogout}>
				<IconButton size="large" aria-label="account of current user" aria-controls="primary-search-account-menu" aria-haspopup="true" color="inherit">
					<LogoutIcon />
				</IconButton>
				<p>Logout</p>
			</MenuItem>
		</Menu>
	);

	// const renderMobileNoAuthMenu = (
	// 	<Menu
	// 		anchorEl={mobileMoreAnchorEl}
	// 		anchorOrigin={{
	// 			vertical: 'top',
	// 			horizontal: 'right',
	// 		}}
	// 		id={mobileMenuId}
	// 		keepMounted
	// 		transformOrigin={{
	// 			vertical: 'top',
	// 			horizontal: 'right',
	// 		}}
	// 		open={isMobileMenuOpen}
	// 		onClose={handleMobileMenuClose}
	// 	>
	// 		{
	// 			// 	<MenuItem onClick={() => loginWithRedirect()}>
	// 			// 	<IconButton size="large" aria-label="show 4 new mails" color="inherit">
	// 			// 		<LoginIcon />
	// 			// 	</IconButton>
	// 			// 	<p>Login</p>
	// 			// </MenuItem>
	// 			// <MenuItem
	// 			// 	onClick={() =>
	// 			// 		loginWithRedirect({
	// 			// 			authorizationParams: {
	// 			// 				screen_hint: 'signup',
	// 			// 			},
	// 			// 		})
	// 			// 	}
	// 			// >
	// 			// 	<IconButton size="large" aria-label="show 17 new notifications" color="inherit">
	// 			// 		<AssignmentIndIcon />
	// 			// 	</IconButton>
	// 			// 	<p>Signup</p>
	// 			// </MenuItem>
	// 		}
	// 	</Menu>
	// );

	const renderMobileNoAuthMenu = <SelfAuthentication modalOpen={isMobileMenuOpen} handleMenuClose={handleMobileMenuClose} loginCallback={setUser} />;

	return (
		<Box sx={{ flexGrow: 1, width: '100%', marginBottom: '10px' }}>
			<AppBar position="static">
				<Toolbar>
					<Link to="/" style={{ textDecoration: 'none', color: 'white' }} reloadDocument={true}>
						<Typography variant="h4" noWrap component="div" sx={{ display: { xs: 'none', sm: 'block' } }}>
							MiniTwit
						</Typography>
					</Link>
					<Box sx={{ flexGrow: 1 }} />
					<IconButton size="large" edge="end" aria-label="account of current user" aria-controls={menuId} aria-haspopup="true" onClick={handleProfileMenuOpen} color="inherit">
						<AccountCircle />
					</IconButton>
				</Toolbar>
			</AppBar>
			{user.isLoggedIn ? renderMobileAuthMenu : renderMobileNoAuthMenu}
			{user.isLoggedIn ? renderAuthMenu : renderNoAuthMenu}
		</Box>
	);
};

export default Header;
