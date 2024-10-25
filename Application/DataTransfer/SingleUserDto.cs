using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DataTransfer
{
    public class SingleUserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string ProfilePicture { get; set; }
        public string RoleName { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
        public int PostsCount { get; set; }
        public IEnumerable<UserUseCaseDto> UserUseCases { get; set; }
        public IEnumerable<LikeCommentDto> CommentLikes { get; set; }
        public IEnumerable<GetPostDto> UserPosts { get; set; }
        public IEnumerable<CommentDto> UserComments { get; set; }
    }
}