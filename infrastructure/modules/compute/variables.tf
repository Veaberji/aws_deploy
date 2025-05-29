variable "app_name" {
  description = "App name used for tags"
  type        = string
}

variable "volume_size" {
  description = "Size in GB for root volume"
  type        = number
}

variable "ami" {
  description = "AMI ID for EC2"
  type        = string
}

variable "instance_type" {
  description = "EC2 instance type"
  type        = string
}

variable "security_groups" {
  description = "Security groups for EC2"
  type        = list(string)
}

variable "user_data" {
  description = "User data to be run on EC2 launch"
  type        = string
}
