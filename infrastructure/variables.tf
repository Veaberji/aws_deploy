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

variable "volume_size" {
  description = "Size in GB for root volume"
  type        = number
  default     = 8
}

variable "ssh_ingress_cidr" {
  description = "Ips allowed to SSH into EC2"
  type        = list(string)
  default     = ["0.0.0.0/0"] # I'll leave it for now, as github has lots of ips for runners
}

variable "api_port" {
  description = "Port used by app API"
  type        = number
  default     = 7093
}

variable "backend_bucket_name" {
  description = "S3 backend bucket name"
  type        = string
  default     = "musicians-app-tf-state"
}

variable "notification_emails" {
  description = "List of email addresses for SNS notifications"
  type        = list(string)
  default     = ["ec.app.testing@gmail.com"]
}
