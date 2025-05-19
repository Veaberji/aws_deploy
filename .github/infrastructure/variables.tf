variable "region" {
  description = "Default AWS region"
  type        = string
  default     = "us-east-1"
}

variable "ami" {
  description = "Amazon machine image to use for EC2 instance"
  type        = string
  default     = "ami-0953476d60561c955" # Amazon Linux 2023 2025-05-09
}

variable "instance_type" {
  description = "EC2 instance type"
  type        = string
  default     = "t2.micro"
}
