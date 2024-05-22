﻿using Demo.DAL.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;

namespace Demo.PL.ViewModels
{
	public class SignUpViewModel
	{
		[Required(ErrorMessage ="Email is Required")]
		[EmailAddress(ErrorMessage ="Invalid Email")]
		public string Email { get; set; }


		[Required(ErrorMessage = "First Name is Required!!")]
		public string FirstName { get; set; }


		[Required(ErrorMessage = "Last Name is Required!!")]
		public string LastName { get; set; }


		[Required(ErrorMessage = "UserName is Required!!")]
		public string UserName { get; set; }

		[Required(ErrorMessage ="Password is Required")]
		[MinLength(5,ErrorMessage ="Minimum Password Length is 5")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required(ErrorMessage = "Confirm Password is Required")]
		[Compare(nameof(Password), ErrorMessage = "Confirm Password does not match Password")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }

		public bool IsAgree { get; set; }
		
	}
}
