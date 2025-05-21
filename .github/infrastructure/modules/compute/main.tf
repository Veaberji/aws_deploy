resource "aws_instance" "app_server" {
  ami             = var.ami
  instance_type   = var.instance_type
  security_groups = var.security_groups
  user_data       = var.user_data

  root_block_device {
    volume_size           = var.volume_size
    volume_type           = "gp3"
    delete_on_termination = true

    tags = {
      Name = "${var.app_name}-volume"
    }
  }

  tags = {
    Name = "${var.app_name}-ec2"
  }
}
